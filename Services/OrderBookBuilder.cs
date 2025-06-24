using SkyQuant.Models;

namespace SkyQuant.Services;

public class OrderBookBuilder : IOrderBookBuilder
{
    private readonly Dictionary<ulong, Order> orders = [];
    private readonly SortedDictionary<int, (int count, int qty)> bids = new(Comparer<int>.Create((a, b) => b.CompareTo(a)));
    private readonly SortedDictionary<int, (int count, int qty)> asks = [];

    public IReadOnlyList<Snapshot> GetOrderBookRepresentation(IReadOnlyList<Tick> parsed)
    {
        int? bestBid = null, bestAsk = null;
        var snapshots = new List<Snapshot>(parsed.Count);

        foreach (var tick in parsed)
        {
            switch (tick.Action)
            {
                case "F":
                case "Y":
                    orders.Clear(); 
                    bids.Clear(); 
                    asks.Clear();

                    bestBid = bestAsk = null;
                    break;
                case "A":
                    orders[tick.OrderId] = new Order(tick.Price, tick.Qty, tick.Side);

                    var bookA = tick.Side == 1 ? bids : asks;

                    if (!bookA.TryGetValue(tick.Price, out var levA))
                        bookA[tick.Price] = (1, tick.Qty);
                    else
                        bookA[tick.Price] = (levA.count + 1, levA.qty + tick.Qty);

                    if (tick.Side == 1)
                        bestBid = bestBid == null || tick.Price > bestBid ? tick.Price : bestBid;
                    else
                        bestAsk = bestAsk == null || tick.Price < bestAsk ? tick.Price : bestAsk;

                    break;
                case "D":
                    if (!orders.TryGetValue(tick.OrderId, out var od)) break;

                    orders.Remove(tick.OrderId);

                    var bookD = od.Side == 1 ? bids : asks;
                    var levD = bookD[od.Price];

                    if (levD.count == 1)
                        bookD.Remove(od.Price);
                    else
                        bookD[od.Price] = (levD.count - 1, levD.qty - od.Qty);

                    if (od.Side == 1 && bestBid == od.Price && !bookD.ContainsKey(od.Price))
                        bestBid = bids.Count > 0 ? bids.First().Key : null;
                    else if (od.Side == 2 && bestAsk == od.Price && !bookD.ContainsKey(od.Price))
                        bestAsk = asks.Count > 0 ? asks.First().Key : null;

                    break;
                case "M":
                    bool isUpdate = orders.TryGetValue(tick.OrderId, out var old);

                    if (isUpdate)
                    {
                        if (old.Price == tick.Price && old.Qty == tick.Qty && old.Side == tick.Side)
                            break;

                        var bookOld = old.Side == 1 ? bids : asks;
                        var levOld = bookOld[old.Price];

                        if (levOld.count == 1)
                            bookOld.Remove(old.Price);
                        else
                            bookOld[old.Price] = (levOld.count - 1, levOld.qty - old.Qty);

                        if (old.Side == 1 && bestBid == old.Price && !bookOld.ContainsKey(old.Price))
                            bestBid = bids.Count > 0 ? bids.First().Key : null;
                        else if (old.Side == 2 && bestAsk == old.Price && !bookOld.ContainsKey(old.Price))
                            bestAsk = asks.Count > 0 ? asks.First().Key : null;
                    }

                    orders[tick.OrderId] = new Order(tick.Price, tick.Qty, tick.Side);

                    var bookM = tick.Side == 1 ? bids : asks;

                    if (!bookM.TryGetValue(tick.Price, out var levM))
                        bookM[tick.Price] = (1, tick.Qty);
                    else
                        bookM[tick.Price] = (levM.count + 1, levM.qty + tick.Qty);

                    if (tick.Side == 1)
                        bestBid = bestBid == null || tick.Price > bestBid ? tick.Price : bestBid;
                    else
                        bestAsk = bestAsk == null || tick.Price < bestAsk ? tick.Price : bestAsk;
                    break;
            }

            int? B0 = bestBid;
            int? A0 = bestAsk;
            int? BQ0 = B0.HasValue ? bids[B0.Value].qty : null;
            int? BN0 = B0.HasValue ? bids[B0.Value].count : null;
            int? AQ0 = A0.HasValue ? asks[A0.Value].qty : null;
            int? AN0 = A0.HasValue ? asks[A0.Value].count : null;

            snapshots.Add(new Snapshot(tick.RawLine, B0, BQ0, BN0, A0, AQ0, AN0));
        }

        return snapshots;
    }
}
