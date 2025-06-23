
using SkyQuant.View;

namespace SkyQuant.Services
{
    public interface IDataWriter
    {
        void WriteFile(string path, IOrderBookRepresentation fileItems);
    }
}