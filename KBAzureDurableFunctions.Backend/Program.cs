using KBAzureDurableFunctions.Backend.Persistence;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddDbContext<RestaurantDbContext>();
// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

var functionApp = builder.Build();

using var scope = functionApp.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

functionApp.Run();