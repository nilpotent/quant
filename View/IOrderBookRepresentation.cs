
namespace SkyQuant.View
{
    public interface IOrderBookRepresentation
    {
        IList<string> Items { get; }

        void Add(string line, int? B0, int? BQ0, int? BN0, int? A0, int? AQ0, int? AN0);
    }
}