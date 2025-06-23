namespace SkyQuant.View
{
    public class OrderBookRepresentation : IOrderBookRepresentation
    {
        public IList<string> Items { get; private set; } = ["SourceTime,Side,Action,OrderId,Price,Qty,B0,BQ0,BN0,A0,AQ0,AN0"];

        public void Add(string line, int? B0, int? BQ0, int? BN0, int? A0, int? AQ0, int? AN0)
        {
            Items.Add($"{line},{B0?.ToString() ?? ""},{BQ0?.ToString() ?? ""},{BN0?.ToString() ?? ""},{A0?.ToString() ?? ""},{AQ0?.ToString() ?? ""},{AN0?.ToString() ?? ""}");
        }
    }
}
