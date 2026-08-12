using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        // Inject IUserRepository thay vì AppDbContext
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // Gọi qua Repository để lấy dữ liệu
            return await _userRepository.GetAllUsersAsync(cancellationToken);
        }
    }
}
