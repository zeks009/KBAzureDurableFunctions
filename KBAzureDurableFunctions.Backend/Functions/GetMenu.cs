using KBAzureDurableFunctions.Backend.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KBAzureDurableFunctions.Backend.Functions;

/// <summary>
/// Get the menu items for display in the frontend.
/// </summary>
public class GetMenu(ILogger<GetMenu> logger, RestaurantDbContext dbContext)
{
    [Function("GetMenu")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        logger.LogInformation("Getting menu items");
        var menu = dbContext.MenuItems
            .OrderBy(x => x.Group)
            .ThenBy(x => x.Name)
            .ToArray();
        return new OkObjectResult(menu);
    }
}