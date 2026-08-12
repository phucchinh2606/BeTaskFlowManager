using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public SystemRole SystemRole { get; set; }
    }
}
