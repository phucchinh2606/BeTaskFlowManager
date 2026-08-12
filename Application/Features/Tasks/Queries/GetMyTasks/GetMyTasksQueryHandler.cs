using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.GetMyTasks
{
    public class GetMyTasksQueryHandler : IRequestHandler<GetMyTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetMyTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetMyTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByAssigneeIdAsync(request.UserId, cancellationToken);

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                ProjectId = t.ProjectId,
                ProjectName = t.Project?.Name,
                AssigneeId = t.AssigneeId,
                AssigneeName = t.Assignee?.FullName
            });
        }
    }
}
