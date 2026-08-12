using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Khởi tạo thực thể TaskItem
            var task = new TaskItem
            {
                ProjectId = request.ProjectId,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                DueDate = request.DueDate,
                AssigneeId = request.AssigneeId,
                Status = Domain.Enums.TaskStatus.ToDo,
                CreatedAt = DateTime.UtcNow
            };

            // Lưu vào DB
            await _taskRepository.AddAsync(task);
            await _cacheService.RemoveAsync($"ProjectStats_{request.ProjectId}");

            // Trả về Id của task vừa tạo
            return task.Id;
        }
    }
}
