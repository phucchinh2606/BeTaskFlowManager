namespace Application.Features.Projects.Queries.GetProjectDetails
{
    public class ProjectDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsArchived { get; set; }
        public Guid CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProjectMemberDto> Members { get; set; } = new();
        public ProjectTaskSummaryDto TaskSummary { get; set; } = new();
        public List<ProjectTaskBriefDto> RecentTasks { get; set; } = new();
    }

    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProjectRole { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public int AssignedTaskCount { get; set; }
    }

    public class ProjectTaskSummaryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double ProgressPercentage { get; set; }
        public Dictionary<string, int> StatusCounts { get; set; } = new();
    }

    public class ProjectTaskBriefDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public string? AssigneeName { get; set; }
    }
}
