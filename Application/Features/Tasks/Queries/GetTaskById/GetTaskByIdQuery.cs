using Application.Features.Tasks.Queries.GetAllTasks;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public Guid Id { get; set; }
    }
}
