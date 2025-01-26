using Microsoft.AspNetCore.Mvc;
using Report.Core.Models;
using Report.Service.Services;

namespace Report.API.Controllers;

public class ReportsController: ControllerBase
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // Получить отчеты по периоду
        [HttpGet("period")]
        public async Task<ActionResult<List<ReportEntity>>> GetReportsByPeriod([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var reports = await _reportService.GenerateReportByPeriodAsync(
                startDate ?? DateTime.MinValue,
                endDate ?? DateTime.MaxValue
            );
            
            return Ok(reports);
        }
    }
}