using KBAzureDurableFunctions.Backend.Persistence;
using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

/// <summary>
/// This activity is responsible for preparing an item in the order. Usually all items are prepared in parallel.
/// </summary>
public class PrepareItemActivity(RestaurantDbContext db)
{
    [Function(nameof(PrepareItemActivity))]
    public async Task Run([ActivityTrigger] OrderItem orderItem, FunctionContext context)
    {
        var logger = context.GetLogger<FoodOrderingWorkflow>();
        var menuItem = await db.MenuItems.FindAsync(orderItem.MenuItemId);
        logger.LogInformation("Preparing item {itemName}", menuItem);
        var preparationDuration = new Random().Next(0, 30);
        await Task.Delay(preparationDuration * 1000);
        logger.LogInformation("Item {itemName} is prepared in {minutes} minutes", menuItem, preparationDuration);
    }
}