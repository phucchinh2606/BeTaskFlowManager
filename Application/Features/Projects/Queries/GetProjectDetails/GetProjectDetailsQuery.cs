using MediatR;

namespace Application.Features.Projects.Queries.GetProjectDetails
{
    public class GetProjectDetailsQuery : IRequest<ProjectDetailsDto>
    {
        public Guid ProjectId { get; set; }
    }
}
