using Domain.Enums;
using MediatR;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public DateTime? DueDate { get; set; }

        // ID của người được giao việc. Cho phép null nếu tạo task mà chưa phân công ngay.
        public Guid? AssigneeId { get; set; }
    }
}
