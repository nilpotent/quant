namespace SkyQuant.Models;

public readonly struct Order(int price, int qty, byte side)
{
    public readonly int Price = price;

    public readonly int Qty = qty;

    public readonly byte Side = side;
}
