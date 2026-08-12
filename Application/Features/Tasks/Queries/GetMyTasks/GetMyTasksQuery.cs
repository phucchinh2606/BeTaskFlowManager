using Application.Features.Tasks.Queries.GetAllTasks;
using MediatR;

namespace Application.Features.Tasks.Queries.GetMyTasks
{
    public class GetMyTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid UserId { get; set; }

        public GetMyTasksQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
