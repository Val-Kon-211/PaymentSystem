using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Transaction.Core.DataAccess;
using Transaction.Core.DataAccess.Repositories;
using Transaction.Service.Interfaces;
using Transaction.Service.Services;
using Transaction.Service.Services.RabbitMq;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(connectionString));

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

services.AddTransient<ITransactionRepository, TransactionRepository>();
services.AddTransient<IOutboxMessageRepository, IOutboxMessageRepository>();
services.AddTransient<IMessageProducerService, MessageProducerService>();
services.AddTransient<IOutboxService, OutboxService>();
services.AddTransient<ITransactionService, TransactionService>();

services.AddHostedService<TransactionConsumer>();
services.AddHostedService<TransactionPublisher>();