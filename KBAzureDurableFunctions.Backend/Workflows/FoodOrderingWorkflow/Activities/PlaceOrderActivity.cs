using KBAzureDurableFunctions.Backend.Persistence;
using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;

/// <summary>
/// This activity is responsible for placing the order in the database.
/// </summary>
public class PlaceOrderActivity(RestaurantDbContext db)
{
    [Function(nameof(PlaceOrderActivity))]
    public async Task Run([ActivityTrigger] Order order, FunctionContext context)
    {
        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }
}