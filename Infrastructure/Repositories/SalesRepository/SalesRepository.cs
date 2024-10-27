using Microsoft.EntityFrameworkCore;
using SchedulingReportingMicroservice.Domain.Data;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.SalesRepository
{
    public class SalesRepository : GenericRepository<Sales>, ISalesRepository
    {
        public SalesRepository(AppDbContext context) : base(context) { }

        public async Task<decimal> GetTotalSalesAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _context.Sales
                .Where(s => s.Date >= startDate && s.Date <= endDate)
                .SumAsync(s => s.Amount);
        }
    }
}
