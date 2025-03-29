using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

/// <summary>
/// This activity is responsible for preparing the delivery.
/// </summary>
public class PrepareDeliveryActivity
{
    [Function(nameof(PrepareDeliveryActivity))]
    public async Task Run([ActivityTrigger] Order order, FunctionContext context)
    {
        var logger = context.GetLogger<FoodOrderingWorkflow>();
        logger.LogInformation("Preparing delivery");
        await Task.Delay(3000);
        logger.LogInformation("Delivery is prepared");
    }
}