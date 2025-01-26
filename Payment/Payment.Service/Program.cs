using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Core.DataAccess;
using Payment.Core.DataAccess.Repositories;
using Payment.Service.Interfaces;
using Payment.Service.Services;
using Payment.Service.Services.RabbitMq;
using RabbitMQ.Client;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<PaymentDbContext>(options =>
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


services.AddTransient<IPaymentRepository, PaymentRepository>();
services.AddTransient<IOutboxMessageRepository, OutboxMessageRepository>();
services.AddTransient<IPaymentService, PaymentService>();
services.AddTransient<IOutboxService, OutboxService>();
services.AddTransient<IMessageProducerService, MessageProducerService>();

services.AddHostedService<PaymentPublisher>();