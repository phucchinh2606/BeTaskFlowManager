using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.FilterTasks
{
    public class FilterTasksQueryHandler : IRequestHandler<FilterTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public FilterTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(FilterTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.FilterTasksAsync(
                request.Status,
                request.AssigneeId,
                request.Priority,
                cancellationToken);

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
