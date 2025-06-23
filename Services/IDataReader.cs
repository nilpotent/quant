
namespace SkyQuant.Services
{
    public interface IDataReader
    {
        IReadOnlyList<string>? ReadFile(string path);
    }
}