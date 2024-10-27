using Microsoft.EntityFrameworkCore;
using SchedulingReportingMicroservice.Domain.Data;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.OrdersRepository
{
    public class OrdersRepository : GenericRepository<Orders>, IOrdersRepository
    {
        public OrdersRepository(AppDbContext context) : base(context) { }

        public async Task<dynamic> GetOrderStatsAsync(DateTime? startDate, DateTime? endDate)
        {
            return new
            {
                Placed = await _context.Orders.CountAsync(o => o.Status == "Placed" && o.OrderDate >= startDate && o.OrderDate <= endDate),
                Shipped = await _context.Orders.CountAsync(o => o.Status == "Shipped" && o.OrderDate >= startDate && o.OrderDate <= endDate),
                Delivered = await _context.Orders.CountAsync(o => o.Status == "Delivered" && o.OrderDate >= startDate && o.OrderDate <= endDate)
            };
        }
    }
}
