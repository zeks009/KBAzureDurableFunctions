using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace KBAzureDurableFunctions.Backend.Functions;

public class Negotiate
{
    [Function(nameof(Negotiate))]
    public static string Run([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "messages")] string connectionInfo)
    {
        // The serialization of the connection info object is done by the framework. It should be camel case. The SignalR client respects the camel case response only.
        return connectionInfo;
    }
}