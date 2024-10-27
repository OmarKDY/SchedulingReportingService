using Microsoft.AspNetCore.Mvc;
using SchedulingReportingService.Domain.Dtos;
using SchedulingReportingService.Services;
using System.Text;
using System.Linq;

namespace SchedulingReportingService.Controllers
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
        #region Get Report
        [HttpGet]
        public async Task<IActionResult> GetReport([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string source = null, [FromQuery] string url = null, [FromQuery] string apiKey = null)
        {
            var report = await _reportService.CreateReportAsync(start, end, source, url, apiKey);
            return Ok(report);
        }
        #endregion
        #region Export report
        [HttpGet("export")]
        public async Task<IActionResult> ExportReport([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string source = null, [FromQuery] string url = null, [FromQuery] string apiKey = null)
        {
            var report = await _reportService.CreateReportAsync(start, end, source, url, apiKey);
            var csv = GenerateCsv(report);

            var byteArray = System.Text.Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/csv", $"report_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        }

        private string GenerateCsv(ReportDto report)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Total Sales, New Users, Orders");

            if (report.Orders != null && report.Orders.Any())
            {
                var orderIds = ((IEnumerable<OrderDto>)report.Orders)
                            .Select(o => o.OrderId.ToString())
                            .ToArray();
                csvBuilder.AppendLine($"{report.TotalSales}, {report.NewUsers}, {string.Join(";", orderIds)}");
            }
            else
            {
                csvBuilder.AppendLine($"{report.TotalSales}, {report.NewUsers}, No Orders");
            }

            return csvBuilder.ToString();
        }
        #endregion
    }
}
