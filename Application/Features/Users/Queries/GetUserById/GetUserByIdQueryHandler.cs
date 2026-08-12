using Application.Features.Users.Queries.GetUsers;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new Exception($"Không tìm thấy người dùng với ID: {request.Id}");
            }

            // Map sang DTO, tuyệt đối không trả về PasswordHash
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                SystemRole = user.SystemRole.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
