using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            // Sử dụng hàm GetByIdAsync có sẵn từ IGenericRepository
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
            {
                throw new Exception($"Không tìm thấy công việc với ID: {request.Id}");
            }

            // Map thực thể TaskItem sang DTO
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                AssigneeId = task.AssigneeId,
                AssigneeName = task.Assignee?.FullName,
                CreatedAt = task.CreatedAt
            };
        }
    }
}
