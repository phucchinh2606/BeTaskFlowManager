using MediatR;

namespace Application.Features.Users.Commands.DeleteUsers
{
    public class DeleteUsersCommand : IRequest<int>
    {
        public IReadOnlyCollection<Guid> Ids { get; set; } = Array.Empty<Guid>();
    }
}
