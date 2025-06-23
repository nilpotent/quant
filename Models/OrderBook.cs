namespace SkyQuant.Models
{
    public class OrderBook
    {
        private readonly Dictionary<ulong, Order> orders = [];
        private readonly SortedDictionary<int, SortedSet<ulong>> bids = new(new DescendingComparer());
        private readonly SortedDictionary<int, SortedSet<ulong>> asks = [];
        private readonly Dictionary<ulong, int> quantities = [];

        public void Clear()
        {
            orders.Clear();
            bids.Clear();
            asks.Clear();
            quantities.Clear();
        }

        public void Add(in Order order)
        {
            orders[order.OrderId] = order;

            quantities[order.OrderId] = order.Qty;

            var book = order.Side == 1 ? bids : asks;

            if (!book.TryGetValue(order.Price, out var level))
            {
                level = [];
                book[order.Price] = level;
            }

            level.Add(order.OrderId);
        }

        public void Delete(ulong orderId)
        {
            if (!orders.TryGetValue(orderId, out var order))
            {
                return;
            }

            var book = order.Side == 1 ? bids : asks;

            if (book.TryGetValue(order.Price, out var level))
            {
                level.Remove(orderId);

                if (level.Count == 0) 
                { 
                    book.Remove(order.Price); 
                }
            }

            orders.Remove(orderId);
            quantities.Remove(orderId);
        }

        public void Modify(in Order newOrder)
        {
            Delete(newOrder.OrderId);
            Add(in newOrder);
        }

        public (int? B0, int? BQ0, int? BN0, int? A0, int? AQ0, int? AN0) GetBest()
        {
            int? B0 = bids.Count > 0 ? bids.First().Key : null;
            int? A0 = asks.Count > 0 ? asks.First().Key : null;

            int? BQ0 = B0.HasValue ? bids[B0.Value].Sum(oid => quantities[oid]) : null;
            int? AQ0 = A0.HasValue ? asks[A0.Value].Sum(oid => quantities[oid]) : null;
            int? BN0 = B0.HasValue ? bids[B0.Value].Count : null;
            int? AN0 = A0.HasValue ? asks[A0.Value].Count : null;

            return (B0, BQ0, BN0, A0, AQ0, AN0);
        }
    }
}
