using MediatR;
using DomainTaskStatus = Domain.Enums.TaskStatus;

namespace Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand : IRequest<bool>
    {
        public Guid TaskId { get; }
        public DomainTaskStatus Status { get; }
        public Guid UserId { get; }

        public UpdateTaskStatusCommand(Guid taskId, DomainTaskStatus status, Guid userId)
        {
            TaskId = taskId;
            Status = status;
            UserId = userId;
        }
    }
}
