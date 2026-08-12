using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.SearchTasks
{
    public class SearchTasksQueryHandler : IRequestHandler<SearchTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public SearchTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(SearchTasksQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return Enumerable.Empty<TaskDto>();
            }

            var tasks = await _taskRepository.SearchTasksByTitleAsync(request.SearchTerm, cancellationToken);

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
