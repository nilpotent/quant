using SkyQuant.Models;
using SkyQuant.View;

namespace SkyQuant.Services
{
    public class OrderBookBuilder : IOrderBookBuilder
    {
        public IOrderBookRepresentation GetOrderBookRepresentation(IReadOnlyList<string> ticks)
        {
            var orderBook = new OrderBook();
            var fileItems = new OrderBookRepresentation();

            foreach (var line in ticks)
            {
                var parts = line.Split(',');
                var sourceTime = long.Parse(parts[0]);
                var action = parts[2];
                ulong orderId = ulong.TryParse(parts[3], out ulong oid) ? oid : 0UL;
                var price = int.TryParse(parts[4], out var p) ? p : 0;
                var qty = int.TryParse(parts[5], out var q) ? q : 0;
                var side = byte.TryParse(parts[1], out var s) ? s : (byte)0;

                switch (action)
                {
                    case "F":
                    case "Y":
                        orderBook.Clear();
                        break;
                    case "A":
                        orderBook.Add(new Order(oid, price, qty, side));
                        break;
                    case "D":
                        orderBook.Delete(oid);
                        break;
                    case "M":
                        orderBook.Modify(new Order(oid, price, qty, side));
                        break;
                    default:
                        ILogger.Log($"Bad operation: {action}");
                        break;
                }

                var (B0, BQ0, BN0, A0, AQ0, AN0) = orderBook.GetBest();

                if (sourceTime >= 24300006000 && sourceTime <= 53400000000)
                {
                    if (B0.HasValue && A0.HasValue && B0.Value >= A0.Value)
                    {
                        ILogger.Log($"Error in Order Book: SourceTime={sourceTime}, B0={B0}, A0={A0}");
                    }
                }

                fileItems.Add(line.TrimEnd(','), B0, BQ0, BN0, A0, AQ0, AN0);
            }

            return fileItems;
        }
    }
}
