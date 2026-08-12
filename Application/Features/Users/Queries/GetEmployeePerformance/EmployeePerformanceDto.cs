namespace Application.Features.Users.Queries.GetEmployeePerformance
{
    public class EmployeePerformanceDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;

        // Tổng số việc được giao
        public int TotalTasks { get; set; }

        // Khối lượng công việc đang ôm (Status = ToDo hoặc InProgress)
        public int ActiveTasks { get; set; }

        // Số việc đã hoàn thành (Status = Done)
        public int CompletedTasks { get; set; }

        // Số việc trễ hạn (Status != Done và DueDate < Thời gian hiện tại)
        public int OverdueTasks { get; set; }
    }
}
