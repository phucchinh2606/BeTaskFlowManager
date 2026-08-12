using MediatR;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        // Có thể thêm tham số lọc theo ProjectId ở đây nếu muốn lấy danh sách user thuộc 1 dự án cụ thể sau này.
    }
}
