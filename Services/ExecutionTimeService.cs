using Microsoft.Extensions.Logging;
using SkyQuant.Models;
using System.Diagnostics;

namespace SkyQuant.Services;

public class ExecutionTimeService(IOrderBookBuilder orderBookBuilder, ILogger<ExecutionTimeService> logger) : IExecutionTimeService
{
    private readonly IOrderBookBuilder _orderBookBuilder = orderBookBuilder;
    private readonly ILogger<ExecutionTimeService> _logger = logger;

    public IReadOnlyList<Snapshot>? ExecutionTime(IReadOnlyList<Tick> ticks)
    {
        var sw = Stopwatch.StartNew();

        var snapshots = _orderBookBuilder.GetOrderBookRepresentation(ticks);

        sw.Stop();

        _logger.LogInformation($"Total time [us]: {sw.ElapsedMilliseconds * 1000.0:F3}");
        _logger.LogInformation($"Time per tick [us]: {sw.ElapsedMilliseconds * 1000.0 / ticks.Count:F3}");
        _logger.LogInformation(Environment.NewLine);

        return snapshots;
    }
}
