using System.Text.Json.Serialization;

namespace KBAzureDurableFunctions.Shared.Domain;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid MenuItemId { get; set; }
    [JsonIgnore] public MenuItem? MenuItem { get; set; }
    public int Quantity { get; set; }
}