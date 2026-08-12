using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new Exception($"Không tìm thấy người dùng với ID: {request.Id}");
            }

            // Cập nhật thông tin
            user.FullName = request.FullName; //[cite: 5]
            user.SystemRole = request.SystemRole; //[cite: 5]

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
