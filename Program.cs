using Microsoft.Extensions.DependencyInjection;
using SkyQuant;
using SkyQuant.Services;

var services = new ServiceCollection();

services.AddTransient<App>();
services.AddTransient<IDataReader, DataReader>();
services.AddTransient<IDataWriter, DataWriter>();
services.AddTransient<IOrderBookBuilder, OrderBookBuilder>();


var provider = services.BuildServiceProvider();

var app = provider.GetRequiredService<App>();
app.Run();
