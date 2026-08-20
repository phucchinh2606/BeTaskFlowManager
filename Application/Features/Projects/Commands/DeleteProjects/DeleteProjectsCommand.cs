using MediatR;

namespace Application.Features.Projects.Commands.DeleteProjects
{
    public class DeleteProjectsCommand : IRequest<int>
    {
        public IReadOnlyCollection<Guid> Ids { get; set; } = Array.Empty<Guid>();
    }
}