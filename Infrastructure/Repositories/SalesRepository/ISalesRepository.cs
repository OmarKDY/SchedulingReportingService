using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.SalesRepository
{
    public interface ISalesRepository : IGenericRepository<Sales>
    {
        Task<decimal> GetTotalSalesAsync(DateTime? startDate, DateTime? endDate);
    }
}
