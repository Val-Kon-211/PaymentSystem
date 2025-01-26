using Microsoft.EntityFrameworkCore;
using Report.Core.Models;

namespace Report.Core.DataAccess.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ReportDbContext _context;

    public ReportRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReportEntity>> GetDataByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Reports
            .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
            .ToListAsync();
    }

    public async Task<ReportEntity> AddAsync(ReportEntity report)
    {
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }
}