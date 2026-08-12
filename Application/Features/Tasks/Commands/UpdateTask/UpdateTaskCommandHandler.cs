using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            // 1. Lấy Task hiện tại từ Database
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
            {
                throw new Exception($"Không tìm thấy công việc với ID: {request.Id}");
            }

            // 2. Cập nhật các thông tin được phép chỉnh sửa
            task.Title = request.Title;
            task.Description = request.Description;
            task.Priority = request.Priority;
            task.DueDate = request.DueDate;
            task.AssigneeId = request.AssigneeId;

            // 3. Cập nhật thời gian chỉnh sửa cuối cùng
            task.UpdatedAt = DateTime.UtcNow;

            // 4. Lưu thay đổi xuống Database
            await _taskRepository.UpdateAsync(task);

            // SỬA Ở ĐÂY: Lấy ProjectId từ entity task vừa lấy lên từ DB
            await _cacheService.RemoveAsync($"ProjectStats_{task.ProjectId}");

            return true;
        }
    }
}
