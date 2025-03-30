using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace KBAzureDurableFunctions.Backend.Functions;

public class NotifyUser
{
    private const string HubName = "messages";

    [Function("negotiate")]
    public static IActionResult Negotiate(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
        [SignalRConnectionInfoInput(HubName = HubName)] SignalRConnectionInfo connectionInfo)
    {
        return new OkObjectResult(connectionInfo);
    }
    
    [Function("NotifyUser")]
    [SignalROutput(HubName = HubName)]
    public SignalRMessageAction Run([ActivityTrigger] string message)
    {
        return new SignalRMessageAction("newMessage")
        {
            // broadcast to all the connected clients without specifying any connection, user or group.
            Arguments = new[] { message },
        };
    }
}