using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

public class DeliverOrderActivity
{
    [Function(nameof(DeliverOrderActivity))]
    public async Task Run(
        [ActivityTrigger] Order order, 
        FunctionContext context)
    {
        var logger = context.GetLogger<FoodOrderingWorkflow>();
        logger.LogInformation("Awaiting for delivery to be completed.");
    }
}