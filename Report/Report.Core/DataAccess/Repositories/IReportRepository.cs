using Report.Core.Models;

namespace Report.Core.DataAccess.Repositories;

public interface IReportRepository
{
    Task<List<ReportEntity>> GetDataByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<ReportEntity> AddAsync(ReportEntity report);
}