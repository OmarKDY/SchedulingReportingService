using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;
using SchedulingReportingService.Infrastructure.Repositories.OrdersRepository;
using SchedulingReportingService.Infrastructure.Repositories.SalesRepository;
using SchedulingReportingService.Infrastructure.Repositories.UsersRepository;

namespace SchedulingReportingService.Infrastructure.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        ISalesRepository Sales { get; }
        IUsersRepository Users { get; }
        IOrdersRepository Orders { get; }
        IGenericRepository<ScheduledTasks> ScheduledTasks { get; }
        IGenericRepository<ReportHistory> ReportHistory { get; }
        Task<int> SaveAsync();
    }
}
