using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkyQuant.Models;
using SkyQuant.Services;

namespace SkyQuant;

public class App(IDataReader dataReader, IDataWriter dataWriter, IServiceProvider serviceProvider, IOptions<Settings> options, ILogger<App> logger)
{
    private readonly IDataReader _dataReader = dataReader;
    private readonly IDataWriter _dataWriter = dataWriter;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly Settings _settings = options.Value;
    private readonly ILogger<App> _logger = logger;

    public void Run()
    {
        var ticks = _dataReader.ReadFile(_settings.InputFile);

        if (ticks == null)
        {
            _logger.LogInformation("There is no data");
            return;
        }

        IReadOnlyList<Snapshot>? snapshots = null;

        for (int iteration = 0; iteration < _settings.NumberOfIterations; iteration++)
        {
            var executionTimeService = _serviceProvider.GetRequiredService<IExecutionTimeService>();
            snapshots = executionTimeService.ExecutionTime(ticks);
        }

        if (snapshots != null)
        {
            _dataWriter.WriteFile(_settings.OutputFile, snapshots);
        }
    }
}
