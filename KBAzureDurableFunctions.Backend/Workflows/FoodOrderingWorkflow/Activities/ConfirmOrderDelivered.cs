using KBAzureDurableFunctions.Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

public class ConfirmOrderDelivered(ILogger<ConfirmOrderDelivered> logger, RestaurantDbContext db)
{
    [Function("ConfirmOrderDelivered")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{orderId}/confirm")] HttpRequest req, [DurableClient] DurableTaskClient client, Guid orderId)
    {
        logger.LogInformation("Confirming order delivery.");
        var order = await db.Orders.FindAsync(orderId);
        await client.RaiseEventAsync(order.WorkflowId, "order_delivered");
        logger.LogInformation("Order delivered at {time}.", DateTimeOffset.UtcNow);
        return new OkResult();
    }

}