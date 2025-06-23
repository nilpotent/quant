namespace SkyQuant.Models
{
    public readonly struct Order(ulong orderId, int price, int qty, byte side)
    {
        public readonly ulong OrderId = orderId;
        public readonly int Price = price;
        public readonly int Qty = qty;
        public readonly byte Side = side;
    }
}
