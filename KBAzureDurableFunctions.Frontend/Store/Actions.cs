using KBAzureDurableFunctions.Shared.Domain;

namespace KBAzureDurableFunctions.Frontend.Store;

public record AddItemAction(OrderItem Item);