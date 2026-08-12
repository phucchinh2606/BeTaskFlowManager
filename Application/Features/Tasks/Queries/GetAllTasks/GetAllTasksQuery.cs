using MediatR;

namespace Application.Features.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
    }
}
