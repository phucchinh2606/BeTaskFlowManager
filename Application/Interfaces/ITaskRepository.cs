using Application.Features.Tasks.Queries.GetAllTasks;
using Domain.Entities;
using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        // Lấy danh sách Task của 1 dự án cụ thể, kèm theo thông tin Người phụ trách (Assignee)
        Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(Guid projectId);

        // Thống kê số lượng Task theo trạng thái trong 1 dự án (Dùng cho Dashboard)
        Task<Dictionary<Domain.Enums.TaskStatus, int>> GetTaskStatsAsync(Guid projectId);

        Task<IEnumerable<TaskDto>> GetAllTasksAsync(CancellationToken cancellationToken = default);

        // Trong Application/Interfaces/ITaskRepository.cs
        Task<Dictionary<string, int>> GetTaskStatisticsByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskItem>> GetTasksByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default);

        // Thêm hàm mới: lấy Task khẩn cấp
        Task<IEnumerable<TaskItem>> GetUrgentTasksAsync(Guid assigneeId, int daysThreshold, CancellationToken cancellationToken = default);

        Task<IEnumerable<TaskItem>> GetAllUrgentTasksAsync(int daysThreshold, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskItem>> SearchTasksByTitleAsync(string searchTerm, CancellationToken cancellationToken = default);

        Task<IEnumerable<TaskItem>> FilterTasksAsync(
        TaskStatus? status,
        Guid? assigneeId,
        PriorityLevel? priority,
        CancellationToken cancellationToken = default);
    }
}
