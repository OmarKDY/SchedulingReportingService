namespace SchedulingReportingService.Domain.Dtos
{
    public class ReportDto
    {
        public decimal TotalSales { get; set; }
        public int NewUsers { get; set; }
        public dynamic Orders { get; set; }
    }
    public class OrderStatsDto
    {
        public int Placed { get; set; }
        public int Shipped { get; set; }
        public int Delivered { get; set; }
    }
}
