using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.OrdersRepository
{
    public interface IOrdersRepository : IGenericRepository<Orders>
    {
        Task<dynamic> GetOrderStatsAsync(DateTime? startDate, DateTime? endDate);
    }
}
