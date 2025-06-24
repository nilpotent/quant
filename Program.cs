using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SkyQuant;
using SkyQuant.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<Settings>(context.Configuration.GetSection("Configuration"));
        services.AddTransient<App>();
        services.AddTransient<IDataReader, DataReader>();
        services.AddTransient<IDataWriter, DataWriter>();
        services.AddTransient<IOrderBookBuilder, OrderBookBuilder>();
        services.AddTransient<IExecutionTimeService, ExecutionTimeService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole(options =>
        {
            options.FormatterName = "minimal";
        });

        logging.AddConsoleFormatter<MinimalConsoleFormatter, ConsoleFormatterOptions>();
        logging.SetMinimumLevel(LogLevel.Debug);
    })
    .Build();

var app = host.Services.GetRequiredService<App>();
app.Run();
