using SkyQuant.Models;

namespace SkyQuant.Services;

public interface IDataReader
{
    IReadOnlyList<Tick>? ReadFile(string path);
}