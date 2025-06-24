using SkyQuant.Models;

namespace SkyQuant.Services;

public class DataReader : IDataReader
{
    public IReadOnlyList<Tick>? ReadFile(string path)
    {
        var parsed = new List<Tick>();

        var lines = File.ReadLines(path).Skip(1);

        foreach (var line in lines)
        {
            var parts = line.Split(';');

            parsed.Add(new Tick(
                long.Parse(parts[0]),
                byte.TryParse(parts[1], out var s) ? s : (byte)0,
                parts[2],
                ulong.TryParse(parts[3], out var oid) ? oid : 0UL,
                int.TryParse(parts[4], out var price) ? price : 0,
                int.TryParse(parts[5], out var qty) ? qty : 0,
                line
            ));
        }

        return parsed;
    }
}
