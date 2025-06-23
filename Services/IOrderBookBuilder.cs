
using SkyQuant.View;

namespace SkyQuant.Services
{
    public interface IOrderBookBuilder
    {
        IOrderBookRepresentation GetOrderBookRepresentation(IReadOnlyList<string> ticks);
    }
}