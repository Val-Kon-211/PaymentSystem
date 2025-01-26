using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Report.Core.DataAccess;
using Report.Core.DataAccess.Repositories;
using Report.Service.Interfaces;
using Report.Service.Services;
using Report.Service.Services.RabbitMq;

var services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<ReportDbContext>(options => options.UseSqlServer(connectionString));

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

services.AddTransient<IReportRepository, ReportRepository>();
services.AddTransient<IReportService, ReportService>();

services.AddHostedService<ReportConsumer>();