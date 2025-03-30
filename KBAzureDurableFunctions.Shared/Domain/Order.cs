namespace KBAzureDurableFunctions.Shared.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
    public string WorkflowId { get; set; }
    
    public int NumberOfItems => Items.Count;
}