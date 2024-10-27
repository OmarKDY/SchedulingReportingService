using System.ComponentModel.DataAnnotations;

namespace SchedulingReportingService.Domain.Entities
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

        public int UserId { get; set; }
        public virtual Users? User { get; set; }

        public virtual ICollection<Sales>? Sales { get; set; } = new List<Sales>();
    }
}
