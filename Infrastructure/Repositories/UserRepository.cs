using Application.Features.Users.Queries.GetEmployeePerformance;
using Application.Features.Users.Queries.GetUsers;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            // Tối ưu hiệu năng đọc bằng AsNoTracking và Select trực tiếp ra DTO ngay tại DB
            return await _dbSet
                .AsNoTracking()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    SystemRole = u.SystemRole.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        public async Task<IEnumerable<EmployeePerformanceDto>> GetEmployeePerformanceAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .AsNoTracking()
                .Select(u => new EmployeePerformanceDto
                {
                    UserId = u.Id,
                    FullName = u.FullName,

                    // Tổng số việc được gán cho User này
                    TotalTasks = u.AssignedTasks.Count(),

                    // Đếm số việc đang làm hoặc chưa làm
                    ActiveTasks = u.AssignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.ToDo || t.Status == Domain.Enums.TaskStatus.InProgress),

                    // Đếm số việc đã xong
                    CompletedTasks = u.AssignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.Done),

                    // Đếm số việc trễ deadline (Chưa Done và có DueDate nhỏ hơn thời gian hiện tại)
                    OverdueTasks = u.AssignedTasks.Count(t => t.Status != Domain.Enums.TaskStatus.Done && t.DueDate != null && t.DueDate < now)
                })
                // Sắp xếp ưu tiên hiển thị những người trễ deadline nhiều nhất lên đầu, sau đó đến những người ôm nhiều việc nhất
                .OrderByDescending(x => x.OverdueTasks)
                .ThenByDescending(x => x.ActiveTasks)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task DeleteRangeAsync(IEnumerable<User> users, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(users);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
