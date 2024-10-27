using Microsoft.EntityFrameworkCore;
using SchedulingReportingService.Domain.Entities;

namespace SchedulingReportingService.Domain.Seeding
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(new Users { UserId = 1, Name = "Omar", RegisteredDate = DateTime.Now });
            modelBuilder.Entity<Sales>().HasData(new Sales { SaleId = 1, Amount = 1000, Date = DateTime.Now });
            modelBuilder.Entity<Orders>().HasData(new Orders { OrderId = 1, Status = "Placed", OrderDate = DateTime.Now });
        }
    }
}
