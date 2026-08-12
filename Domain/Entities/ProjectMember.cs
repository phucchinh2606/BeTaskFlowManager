using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectRole ProjectRole { get; set; } = ProjectRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Project Project { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}