using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra Task có tồn tại trong hệ thống hay không
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Không tìm thấy công việc.");
            }

            // 2. Kiểm tra phân quyền: Chỉ Assignee mới được phép cập nhật tiến độ
            if (task.AssigneeId != request.UserId)
            {
                throw new UnauthorizedAccessException("Bạn không được phân công xử lý công việc này.");
            }

            // 3. Cập nhật trạng thái và thời gian chỉnh sửa
            task.Status = (Domain.Enums.TaskStatus)request.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);
            return true;
        }
    }
}
