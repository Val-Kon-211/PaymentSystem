using Report.Core.DataAccess.Repositories;
using Report.Core.Models;
using Report.Service.Events;
using Report.Service.Interfaces;

namespace Report.Service.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }
    
    public async Task HandleTransactionCreatedEvent(TransactionCreatedEvent @event)
    {
        // Обновляем данные для отчетов
        var reportData = new
        {
            TransactionId = @event.TransactionId,
            PaymentId = @event.PaymentId,
            Status = @event.Status,
            CreatedAt = @event.CreatedAt
        };

        var report = new ReportEntity
        {
            Content = System.Text.Json.JsonSerializer.Serialize(reportData),
            StartDate = DateTime.UtcNow.AddDays(-1), // Пример периода
            EndDate = DateTime.UtcNow
        };

        await _reportRepository.AddAsync(report);
    }
    
    public async Task<List<ReportEntity>> GenerateReportByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _reportRepository.GetDataByPeriodAsync(startDate, endDate);
    }
}