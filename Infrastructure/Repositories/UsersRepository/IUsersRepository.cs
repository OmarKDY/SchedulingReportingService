using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.UsersRepository
{
    public interface IUsersRepository : IGenericRepository<Users>
    {
        Task<int> GetNewUsersCountAsync(DateTime? startDate, DateTime? endDate);
    }
}
