using SkyQuant.Models;

namespace SkyQuant.Services;

public class DataWriter : IDataWriter
{
    public void WriteFile(string path, IReadOnlyList<Snapshot> snapshots)
    {
        var output = new List<string>(snapshots.Count + 1)
        {
            "SourceTime;Side;Action;OrderId;Price;Qty;B0;BQ0;BN0;A0;AQ0;AN0"
        };

        foreach (var s in snapshots)
        {
            output.Add($"{s.RawLine};{s.B0?.ToString() ?? ""};{s.BQ0?.ToString() ?? ""};{s.BN0?.ToString() ?? ""};{s.A0?.ToString() ?? ""};{s.AQ0?.ToString() ?? ""};{s.AN0?.ToString() ?? ""}");
        }

        File.WriteAllLines(path, output);
    }
}
