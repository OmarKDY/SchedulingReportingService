using Microsoft.EntityFrameworkCore;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Domain.Seeding;

namespace SchedulingReportingMicroservice.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ScheduledTasks> ScheduledTasks { get; set; }
        public DbSet<ReportHistory> ReportHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData.Seed(modelBuilder);
        }
    }
}
