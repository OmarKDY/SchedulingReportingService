using System.ComponentModel.DataAnnotations;

namespace SchedulingReportingService.Domain.Entities
{
    public class Sales
    {
        [Key]
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public virtual Users? User { get; set; }

        public int OrderId { get; set; }
        public virtual Orders? Order { get; set; }
    }
}
