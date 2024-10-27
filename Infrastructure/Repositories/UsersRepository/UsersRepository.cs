using Microsoft.EntityFrameworkCore;
using SchedulingReportingMicroservice.Domain.Data;
using SchedulingReportingService.Domain.Entities;
using SchedulingReportingService.Infrastructure.Repositories.GenericRepository;

namespace SchedulingReportingService.Infrastructure.Repositories.UsersRepository
{
    public class UsersRepository : GenericRepository<Users>, IUsersRepository
    {
        public UsersRepository(AppDbContext context) : base(context) { }

        public async Task<int> GetNewUsersCountAsync(DateTime? startDate, DateTime? endDate)
        {
            return await _context.Users
                .CountAsync(u => u.RegisteredDate >= startDate && u.RegisteredDate <= endDate);
        }
    }
}
