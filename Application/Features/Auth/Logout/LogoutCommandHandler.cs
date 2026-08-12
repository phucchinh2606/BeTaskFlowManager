using MediatR;

namespace Application.Features.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Do dự án sử dụng JWT dạng Stateless (không lưu RefreshToken trong DB),
            // Handler này đại diện cho việc nhận yêu cầu đăng xuất hợp lệ từ User.
            return Task.FromResult(true);
        }
    }
}
