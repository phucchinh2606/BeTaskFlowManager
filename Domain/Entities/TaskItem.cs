using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        public Guid ProjectId { get; set; }
        public Guid? AssigneeId { get; set; } // Có thể null (chưa giao ai)

        // Navigation Properties
        public Project Project { get; set; } = null!;
        public User? Assignee { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}