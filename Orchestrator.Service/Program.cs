using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orchestrator.Service;
using RabbitMQ.Client;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

services.AddSingleton<IConnection>(_ =>
{
    var factory = new ConnectionFactory
    {
        HostName = configuration["RabbitMQ:Hostname"]?? "localhost",
        UserName = configuration["RabbitMQ:UserName"],
        Password = configuration["RabbitMQ:Password"],
        Port = int.Parse(configuration["RabbitMQ:Port"])
    };
    return factory.CreateConnectionAsync().Result;
});

services.AddHostedService<RabbitMqMessageHandler>();