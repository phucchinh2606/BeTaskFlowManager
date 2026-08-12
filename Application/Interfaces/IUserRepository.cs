using Application.Features.Users.Queries.GetEmployeePerformance;
using Application.Features.Users.Queries.GetUsers;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // Cần kiểm tra xem email đã tồn tại trong DB chưa trước khi tạo mới
        Task<bool> IsEmailUniqueAsync(string email);

        Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<EmployeePerformanceDto>> GetEmployeePerformanceAsync(CancellationToken cancellationToken = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
