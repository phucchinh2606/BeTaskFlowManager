using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsArchived { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
