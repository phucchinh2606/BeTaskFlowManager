using MediatR;

namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }
}
