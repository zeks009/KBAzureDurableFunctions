using KBAzureDurableFunctions.Backend.Functions;
using KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow.Activities;
using KBAzureDurableFunctions.Shared.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace KBAzureDurableFunctions.Backend.Workflows.FoodOrderingWorkflow;

/// <summary>
/// Represents the food ordering workflow.
/// </summary>
public class FoodOrderingWorkflow
{
    /// <summary>
    /// Timeout for the free order reward.
    /// </summary>
    private const double FreeOrderTimeout = 30;

    [Function(nameof(FoodOrderingWorkflow))]
    public async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(FoodOrderingWorkflow));
        logger.LogInformation("Food ordering workflow started.");
        
        using var timeoutCts = new CancellationTokenSource();
        
        var timeout = context.CurrentUtcDateTime.AddSeconds(FreeOrderTimeout);
        var timeoutTask = context.CreateTimer(timeout, timeoutCts.Token);
        
        var order = context.GetInput<Order>();
        order.WorkflowId = context.InstanceId;

        // Place the order in the database.
        await context.CallActivityAsync(nameof(PlaceOrderActivity), order);
        await context.CallActivityAsync(nameof(NotifyUser), "Your order has been received.");

        // Prepare each item in the order, in parallel. => This is a FAN-OUT pattern example.
        var preparationTasks = Task.WhenAll(
            order.Items.Select(x => context.CallActivityAsync(nameof(PrepareItemActivity), x)).ToArray()
        );
        
        // Wait for all items to be prepared. => This is a FAN-IN pattern.
        await Task.WhenAll(preparationTasks);
        await context.CallActivityAsync(nameof(NotifyUser), "Your order has been prepared.");
        
        // Prepare the delivery. => This is a FUNCTION-CHAINING pattern example.
        await context.CallActivityAsync(nameof(PrepareDeliveryActivity), order);
        await context.CallActivityAsync(nameof(NotifyUser), "Your delivery is ready.");

        // Check if there is an available delivery driver. => This is a MONITORING pattern example.
        while(!await context.CallActivityAsync<bool>(nameof(HasAvailableDeliveryDriverActivity), order));
        {
            // Nothing here, just waiting for an available delivery driver. Once driver available, the loop will break.
        }
        
        // Deliver the order. => This is a FUNCTION-CHAINING pattern example.
        await context.CallActivityAsync(nameof(DeliverOrderActivity), order);
        await context.CallActivityAsync(nameof(NotifyUser), "Your order is on the way.");

        // Wait for the order to be delivered. => This is a HUMAN INTERACTION example. We are waiting for an external order_delivered event.
        var confirmedTask = context.WaitForExternalEvent<object>("order_delivered");
        
        // Wait for the order to be delivered or for the timeout to be reached. => This is a TIMEOUT pattern example.
        var outcome = await Task.WhenAny(confirmedTask, timeoutTask);
        if (outcome == timeoutTask)
        {
            // Timeout reached. Customer has rewarded the free meal
            logger.LogInformation("Timeout reached. Customer has rewarded the free meal.");
            await context.CallActivityAsync(nameof(NotifyUser), "You have been rewarded a free meal for your patience.");
        }
        else
        {
            // Order delivered in time. Customer is happy.
            logger.LogInformation("Order delivered in time. Customer is happy.");
            await context.CallActivityAsync(nameof(NotifyUser), "Your order has been delivered. Enjoy your pizza 🍕. Thank you for ordering with us.");
        }

        if (!timeoutTask.IsCompleted)
        {
            // Cancel the timeout task if it's still running.
            timeoutCts.Cancel();
        }
        
        // Workflow completed.
        logger.LogInformation("Food ordering workflow completed.");
    }
}