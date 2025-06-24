using SkyQuant.Models;

namespace SkyQuant.Services;

public interface IDataWriter
{
    void WriteFile(string path, IReadOnlyList<Snapshot> snapshots);
}