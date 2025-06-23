using SkyQuant.View;

namespace SkyQuant.Services
{
    public class DataWriter : IDataWriter
    {
        public void WriteFile(string path, IOrderBookRepresentation fileItems)
        {
            File.WriteAllLines(path, fileItems.Items);
        }
    }
}
