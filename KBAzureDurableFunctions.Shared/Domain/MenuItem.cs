namespace KBAzureDurableFunctions.Shared.Domain;

public class MenuItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    
    public override string ToString()
    {
        return $"{Group} {Name}";
    }
}