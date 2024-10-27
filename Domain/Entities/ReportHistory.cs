using System.ComponentModel.DataAnnotations;

namespace SchedulingReportingService.Domain.Entities
{
    public class ReportHistory
    {
        [Key]
        public int ReportHistoryId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public decimal TotalSales { get; set; }
        public int NewUsers { get; set; }
        public string OrderIds { get; set; }
    }
}
