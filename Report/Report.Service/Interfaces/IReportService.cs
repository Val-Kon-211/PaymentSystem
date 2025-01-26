using Report.Core.Models;
using Report.Service.Events;

namespace Report.Service.Interfaces;

public interface IReportService
{
    Task HandleTransactionCreatedEvent(TransactionCreatedEvent @event);
    
    Task<List<ReportEntity>> GenerateReportByPeriodAsync(DateTime startDate, DateTime endDate);
}