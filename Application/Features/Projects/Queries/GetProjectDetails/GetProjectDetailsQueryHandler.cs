using Application.Interfaces;
using Domain.Enums;
using MediatR;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Features.Projects.Queries.GetProjectDetails
{
    public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetailsDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public GetProjectDetailsQueryHandler(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<ProjectDetailsDto> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetProjectWithMembersAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy dự án với ID: {request.ProjectId}");
            }

            var creator = await _userRepository.GetByIdAsync(project.CreatedById);
            var tasks = (await _taskRepository.GetTasksByProjectIdAsync(project.Id)).ToList();
            var statusCounts = await _taskRepository.GetTaskStatisticsByProjectIdAsync(project.Id, cancellationToken);

            foreach (var status in Enum.GetNames(typeof(TaskStatus)))
            {
                statusCounts.TryAdd(status, 0);
            }

            var completedTasks = statusCounts.GetValueOrDefault(TaskStatus.Done.ToString(), 0);
            var totalTasks = statusCounts.Values.Sum();
            var now = DateTime.UtcNow;

            var assigneeCounts = tasks
                .Where(t => t.AssigneeId.HasValue)
                .GroupBy(t => t.AssigneeId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var members = project.ProjectMembers
                .OrderByDescending(m => m.ProjectRole == ProjectRole.Manager)
                .ThenBy(m => m.User.FullName)
                .Select(m => new ProjectMemberDto
                {
                    UserId = m.UserId,
                    FullName = m.User.FullName,
                    Email = m.User.Email,
                    ProjectRole = m.ProjectRole.ToString(),
                    JoinedAt = m.JoinedAt,
                    AssignedTaskCount = assigneeCounts.GetValueOrDefault(m.UserId, 0)
                })
                .ToList();

            var memberIds = members.Select(m => m.UserId).ToHashSet();
            foreach (var group in tasks.Where(t => t.AssigneeId.HasValue && !memberIds.Contains(t.AssigneeId.Value)).GroupBy(t => t.AssigneeId!.Value))
            {
                var assignee = group.First().Assignee;
                if (assignee == null) continue;

                members.Add(new ProjectMemberDto
                {
                    UserId = assignee.Id,
                    FullName = assignee.FullName,
                    Email = assignee.Email,
                    ProjectRole = "Contributor",
                    JoinedAt = group.Min(t => t.CreatedAt),
                    AssignedTaskCount = group.Count()
                });
            }

            return new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsArchived = project.IsArchived,
                CreatedById = project.CreatedById,
                CreatedByName = creator?.FullName,
                CreatedAt = project.CreatedAt,
                Members = members,
                TaskSummary = new ProjectTaskSummaryDto
                {
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    InProgressTasks = statusCounts.GetValueOrDefault(TaskStatus.InProgress.ToString(), 0),
                    OverdueTasks = tasks.Count(t => t.DueDate.HasValue && t.Status != TaskStatus.Done && t.DueDate.Value < now),
                    ProgressPercentage = totalTasks == 0 ? 0 : Math.Round(completedTasks * 100.0 / totalTasks, 1),
                    StatusCounts = statusCounts
                },
                RecentTasks = tasks
                    .OrderByDescending(t => t.UpdatedAt)
                    .Take(8)
                    .Select(t => new ProjectTaskBriefDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status.ToString(),
                        Priority = t.Priority.ToString(),
                        DueDate = t.DueDate,
                        AssigneeName = t.Assignee?.FullName
                    })
                    .ToList()
            };
        }
    }
}
