using Report.Service.Interfaces;
using Report.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IReportService, ReportService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();