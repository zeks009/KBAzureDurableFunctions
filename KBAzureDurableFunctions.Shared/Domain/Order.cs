namespace KBAzureDurableFunctions.Shared.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
    public string WorkflowId { get; set; }
    
    public int NumberOfItems => Items.Count;
    public decimal Total => Items.Sum(x => x.MenuItem?.Price * x.Quantity ?? 0);
    public void Clear() => Items.Clear();
    public bool IsEmpty => !Items.Any();
}