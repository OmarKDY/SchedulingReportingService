using System.ComponentModel.DataAnnotations;

namespace SchedulingReportingService.Domain.Entities
{
    public class ScheduledTasks
    {
        [Key]
        public int ScheduledTaskId { get; set; }
        public string CronExpression { get; set; }
        public string TaskType { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Source { get; set; }
        public string? Url { get; set; }
        public string? ApiKey { get; set; }

    }
}
