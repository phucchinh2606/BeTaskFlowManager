using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService; // Bổ sung CacheService

        // Inject CacheService qua Constructor
        public DeleteTaskCommandHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            // Lấy Task hiện tại từ Database
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
            {
                throw new Exception($"Không tìm thấy công việc với ID: {request.Id}");
            }

            // Xóa khỏi DB
            await _taskRepository.DeleteAsync(task);

            // BỔ SUNG Ở ĐÂY: Xóa Cache thống kê của dự án chứa Task này
            await _cacheService.RemoveAsync($"ProjectStats_{task.ProjectId}");

            return true;
        }
    }
}
