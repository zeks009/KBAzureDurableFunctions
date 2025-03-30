using Fluxor;
using KBAzureDurableFunctions.Shared.Domain;

namespace KBAzureDurableFunctions.Frontend.Store;

public static class Reducers
{
    [ReducerMethod]
    public static State ReduceAddItemAction(State state, AddItemAction action)
    {
        var order = state.Order;
        order.Items.Add(action.Item);
        return state with { Order = order };
    }
    
    [ReducerMethod]
    public static State ReduceCreateNewOrderAction(State state, CreateNewOrderAction _) => state with { Order = new Order() };
}