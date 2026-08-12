using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    DueDate = t.DueDate,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name,
                    AssigneeId = t.AssigneeId,
                    AssigneeName = t.Assignee != null ? t.Assignee.FullName : null,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(t => t.Project)   // Lấy thông tin Project của TaskItem[cite: 1]
                .Include(t => t.Assignee)  // Lấy thông tin người được gán (User) nếu cần[cite: 1]
                .Where(t => t.AssigneeId == assigneeId) // Lọc theo AssigneeId của TaskItem[cite: 1]
                .OrderByDescending(t => t.CreatedAt)    // Task mới tạo lên đầu[cite: 1]
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _dbSet
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Assignee) // Nạp luôn thông tin người dùng (Eager Loading)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetTaskStatisticsByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            // Gom nhóm (Group By) theo Status và đếm số lượng trực tiếp dưới Database
            return await _dbSet
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId) //[cite: 9]
                .GroupBy(t => t.Status) //[cite: 9]
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);
        }

        public async Task<Dictionary<Domain.Enums.TaskStatus, int>> GetTaskStatsAsync(Guid projectId)
        {
            // Nhóm các Task theo trạng thái và đếm số lượng
            var stats = await _dbSet
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count);

            // Đảm bảo trả về cả những trạng thái có 0 task
            var allStatuses = Enum.GetValues(typeof(Domain.Enums.TaskStatus))
                                  .Cast<Domain.Enums.TaskStatus>();

            var result = new Dictionary<Domain.Enums.TaskStatus, int>();
            foreach (var status in allStatuses)
            {
                result[status] = stats.ContainsKey(status) ? stats[status] : 0;
            }

            return result;
        }

        public async Task<IEnumerable<TaskItem>> GetUrgentTasksAsync(Guid assigneeId, int daysThreshold, CancellationToken cancellationToken = default)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _dbSet
                .AsNoTracking()
                .Include(t => t.Project)
                .Where(t =>
                    t.AssigneeId == assigneeId &&
                    t.Status != Domain.Enums.TaskStatus.Done && // Giả định bạn có enum TaskStatus.Done
                    t.DueDate.HasValue &&
                    t.DueDate.Value <= thresholdDate)
                .OrderBy(t => t.DueDate) // Sắp xếp ngày đến hạn gần nhất/quá hạn lên đầu
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> GetAllUrgentTasksAsync(int daysThreshold, CancellationToken cancellationToken = default)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _dbSet
                .AsNoTracking()
                .Where(t => t.AssigneeId != null
                         && t.Status != Domain.Enums.TaskStatus.Done
                         && t.DueDate.HasValue
                         && t.DueDate.Value <= thresholdDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> SearchTasksByTitleAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var trimmedSearch = searchTerm.Trim().ToLower();

            return await _dbSet
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Where(t => t.Title.ToLower().Contains(trimmedSearch))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>> FilterTasksAsync(
    Domain.Enums.TaskStatus? status,
    Guid? assigneeId,
    PriorityLevel? priority,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .AsQueryable();

            // Lọc theo trạng thái (nếu có truyền vào)
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Lọc theo người được giao (nếu có truyền vào)
            if (assigneeId.HasValue)
            {
                query = query.Where(t => t.AssigneeId == assigneeId.Value);
            }

            // Lọc theo mức độ ưu tiên (nếu có truyền vào)
            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
