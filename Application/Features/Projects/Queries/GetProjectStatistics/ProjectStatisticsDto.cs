using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Projects.Queries.GetProjectStatistics
{
    public class ProjectStatisticsDto
    {
        public Guid ProjectId { get; set; }
        public Dictionary<string, int> StatusCounts { get; set; } = new();
        public int TotalTasks => StatusCounts.Values.Sum(); // Tự động tính tổng
    }
}
