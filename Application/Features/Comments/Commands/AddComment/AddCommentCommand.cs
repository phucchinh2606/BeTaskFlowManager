using MediatR;

namespace Application.Features.Comments.Commands.AddComment
{
    public class AddCommentCommand : IRequest<Guid>
    {
        public Guid TaskId { get; }
        public Guid UserId { get; }
        public string Content { get; }

        public AddCommentCommand(Guid taskId, Guid userId, string content)
        {
            TaskId = taskId;
            UserId = userId;
            Content = content;
        }
    }
}
