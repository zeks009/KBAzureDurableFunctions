using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

/// <summary>
/// This activity is responsible for checking if there is an available delivery driver.
/// </summary>
/// <returns></returns>
public class HasAvailableDeliveryDriverActivity
{
    [Function(nameof(HasAvailableDeliveryDriverActivity))]
    public async Task<bool> Run([ActivityTrigger] Order input, FunctionContext context)
    {
        // Check if there is an available delivery driver.
        var logger = context.GetLogger<FoodOrderingWorkflow>();
        logger.LogInformation("Checking if there is an available delivery driver.");
        await Task.Delay(2000);
        return new Random().Next(0, 2) == 1;
    }
}