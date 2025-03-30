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
    private const double FreeOrderTimeout = 60;
    
    private DateTime? _workflowStartTime;

    [Function(nameof(FoodOrderingWorkflow))]
    public async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(FoodOrderingWorkflow));
        logger.LogInformation("Food ordering workflow started.");

        _workflowStartTime = context.CurrentUtcDateTime;
        
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
        await context.CallActivityAsync(nameof(NotifyUser), $"Your order has been prepared ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
        
        // Prepare the delivery. => This is a FUNCTION-CHAINING pattern example.
        await context.CallActivityAsync(nameof(PrepareDeliveryActivity), order);
        await context.CallActivityAsync(nameof(NotifyUser), $"Your delivery is ready ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec). Awaiting for a delivery driver.");

        // Check if there is an available delivery driver. => This is a MONITORING pattern example.
        while(!await context.CallActivityAsync<bool>(nameof(HasAvailableDeliveryDriverActivity), order));
        {
            // Nothing here, just waiting for an available delivery driver. Once driver available, the loop will break.
        }
        
        // Deliver the order. => This is a FUNCTION-CHAINING pattern example.
        await context.CallActivityAsync(nameof(DeliverOrderActivity), order);
        await context.CallActivityAsync(nameof(NotifyUser), $"Your order is on the way ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");

        // Wait for the order to be delivered. => This is a HUMAN INTERACTION example. We are waiting for an external order_delivered event.
        var elapsedSoFar = (int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds;
        var remainingTimeout = FreeOrderTimeout - elapsedSoFar;
        if (remainingTimeout > 0)
        {
            var timeout = TimeSpan.FromSeconds(remainingTimeout);
            using var timeoutCts = new CancellationTokenSource();
            var timeoutTask = context.CreateTimer(timeout, timeoutCts.Token);
            var confirmedTask = context.WaitForExternalEvent<object>("order_delivered", timeoutCts.Token);

            logger.LogInformation($"Waiting for the order to be delivered or for the timeout of {timeout:ss} sec to be reached.");
            var outcome = await Task.WhenAny(confirmedTask, timeoutTask);
            if (outcome == timeoutTask)
            {
                logger.LogInformation($"Timeout reached. Customer has rewarded the free meal ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
                await context.CallActivityAsync(nameof(NotifyUser), $"You have been rewarded a free meal for your patience \ud83c\udf89. Enjoy your pizza 🍕. ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
            }
            else
            {
                logger.LogInformation("Order delivered in time. Customer is happy.");
                await context.CallActivityAsync(nameof(NotifyUser), $"Your order has been delivered. Enjoy your pizza 🍕. ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
            }

            if (!timeoutTask.IsCompleted)
            {
                timeoutCts.Cancel();
            }
        }
        else
        {
            logger.LogInformation($"Timeout reached. Customer has rewarded the free meal ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
            await context.CallActivityAsync(nameof(NotifyUser), $"You have been rewarded a free meal for your patience \ud83c\udf89. Enjoy your pizza 🍕. ({(int)(context.CurrentUtcDateTime - _workflowStartTime.Value).TotalSeconds} sec).");
        }
        
        // Workflow completed.
        logger.LogInformation("Food ordering workflow completed.");
    }
}