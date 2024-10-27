using SchedulingReportingMicroservice.Domain.Data;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;
using SchedulingReportingService.Infrastructure.Repositories.OrdersRepository;
using SchedulingReportingService.Infrastructure.Repositories.SalesRepository;
using SchedulingReportingService.Infrastructure.Repositories.UsersRepository;

namespace SchedulingReportingService.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ISalesRepository Sales { get; private set; }
        public IUsersRepository Users { get; private set; }
        public IOrdersRepository Orders { get; private set; }
        public IGenericRepository<ScheduledTasks> ScheduledTasks { get; private set; }
        public IGenericRepository<ReportHistory> ReportHistory { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Sales = new SalesRepository(_context);
            Users = new UsersRepository(_context);
            Orders = new OrdersRepository(_context);
            ScheduledTasks = new GenericRepository<ScheduledTasks>(_context);
            ReportHistory = new GenericRepository<ReportHistory>(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
