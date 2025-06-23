using SkyQuant.Services;
using System.Diagnostics;

namespace SkyQuant
{
    public class App(IDataReader dataReader, IDataWriter dataWriter, IOrderBookBuilder orderBookBuilder)
    {
        private readonly IDataReader _dataReader = dataReader;
        private readonly IDataWriter _dataWriter = dataWriter;
        private readonly IOrderBookBuilder _orderBookBuilder = orderBookBuilder;

        public void Run()
        {
            var ticks = _dataReader.ReadFile("ticks.csv");

            if (ticks == null)
            {
                ILogger.Log("There is no data");
                return;
            }

            var sw = Stopwatch.StartNew();

            var fileItems = _orderBookBuilder.GetOrderBookRepresentation(ticks);

            sw.Stop();

            _dataWriter.WriteFile("ticks_result.csv", fileItems);

            ILogger.Log($"Total time [us]: {sw.ElapsedMilliseconds * 1000.0:F3}");
            ILogger.Log($"Time per tick [us]: {sw.ElapsedMilliseconds * 1000.0 / ticks.Count:F3}");
        }
    }

}
