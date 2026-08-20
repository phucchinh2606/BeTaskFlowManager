using Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new Exception($"Không tìm thấy người dùng với ID: {request.Id}");
            }

            if (user.SystemRole == SystemRole.Admin)
            {
                throw new InvalidOperationException("Không được phép xóa tài khoản Admin.");
            }

            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}
