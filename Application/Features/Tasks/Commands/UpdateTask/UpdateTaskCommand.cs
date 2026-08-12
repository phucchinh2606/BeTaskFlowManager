using Domain.Enums;
using MediatR;

namespace Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Mức độ ưu tiên (Low, Medium, High)
        public PriorityLevel Priority { get; set; }

        // Ngày đến hạn
        public DateTime? DueDate { get; set; }

        // Người phụ trách mới (nếu có thay đổi)
        public Guid? AssigneeId { get; set; }
    }
}
