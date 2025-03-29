using Microsoft.Azure.Functions.Worker;

namespace KBAzureDurableFunctions.Backend.Functions;

public static class NotifyUser
{
    [Function("NotifyUser")]
    [SignalROutput(HubName = "messages", ConnectionStringSetting = "SignalRConnection")]
    public static SignalRMessageAction Run([ActivityTrigger] string message)
    {
        return new SignalRMessageAction("newMessage")
        {
            // broadcast to all the connected clients without specifying any connection, user or group.
            Arguments = new[] { message },
        };
    }
}