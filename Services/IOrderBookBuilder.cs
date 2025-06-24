using SkyQuant.Models;

namespace SkyQuant.Services;

public interface IOrderBookBuilder
{
    IReadOnlyList<Snapshot> GetOrderBookRepresentation(IReadOnlyList<Tick> parsed);
}