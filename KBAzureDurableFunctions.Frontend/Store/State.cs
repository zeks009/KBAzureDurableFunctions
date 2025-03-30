using Fluxor;
using KBAzureDurableFunctions.Shared.Domain;

namespace KBAzureDurableFunctions.Frontend.Store;

[FeatureState]
public record State
{
    private State()
    {
    }

    public Order Order { get; init; } = new();
}