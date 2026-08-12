using Application.Features.Projects.Queries.GetAllProjects;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
    }
}
