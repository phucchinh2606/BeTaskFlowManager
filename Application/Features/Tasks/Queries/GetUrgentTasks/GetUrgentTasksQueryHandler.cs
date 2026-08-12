using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.GetUrgentTasks
{
    public class GetUrgentTasksQueryHandler : IRequestHandler<GetUrgentTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetUrgentTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetUrgentTasksQuery request, CancellationToken cancellationToken)
        {
            var urgentTasks = await _taskRepository.GetUrgentTasksAsync(request.UserId, request.DaysThreshold, cancellationToken);

            return urgentTasks.Select(t => new TaskDto
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
