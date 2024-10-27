using System.ComponentModel.DataAnnotations;

namespace SchedulingReportingService.Domain.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime RegisteredDate { get; set; }
        public virtual ICollection<Sales>? Sales { get; set; } = new List<Sales>();

        public virtual ICollection<Orders>? Orders { get; set; } = new List<Orders>();
    }
}
