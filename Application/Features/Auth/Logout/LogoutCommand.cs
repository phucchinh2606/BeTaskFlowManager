using MediatR;

namespace Application.Features.Auth.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public Guid UserId { get; }

        public LogoutCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
