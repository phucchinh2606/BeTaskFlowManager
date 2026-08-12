using MediatR;

namespace Application.Features.Projects.Queries.GetProjectStatistics
{
    public class GetProjectStatisticsQuery : IRequest<ProjectStatisticsDto>
    {
        public Guid ProjectId { get; set; }
    }
}
