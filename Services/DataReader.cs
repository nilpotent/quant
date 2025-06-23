namespace SkyQuant.Services
{
    public class DataReader : IDataReader
    {
        public IReadOnlyList<string>? ReadFile(string path) => File.ReadAllLines(path).Skip(1).ToList();
    }
}
