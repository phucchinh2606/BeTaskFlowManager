namespace Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        // Navigation Properties
        public TaskItem Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}