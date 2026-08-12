using MediatR;

namespace Application.Features.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
