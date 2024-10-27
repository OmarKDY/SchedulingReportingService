namespace SchedulingReportingService.Domain.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
