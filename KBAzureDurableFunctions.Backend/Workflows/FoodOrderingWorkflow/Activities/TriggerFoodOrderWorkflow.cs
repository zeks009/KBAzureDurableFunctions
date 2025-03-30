using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

/// <summary>
/// Here we trigger the food ordering workflow by receiving an order from the customer.
/// </summary>
public class TriggerFoodOrderWorkflow
{
    [Function(nameof(TriggerFoodOrderWorkflow))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]
        HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger(nameof(FoodOrderingWorkflow));
        
        var order = await req.ReadFromJsonAsync<Order>();
        order.Id = Guid.NewGuid();
        logger.LogInformation("Received a new order from customer: {customerId}.", order.CustomerId);

        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(FoodOrderingWorkflow), order);

        logger.LogInformation("Started workflow with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        //return await client.CreateCheckStatusResponseAsync(req, instanceId);

        return new OkObjectResult(new{ OrderId = order.Id });
    }
}