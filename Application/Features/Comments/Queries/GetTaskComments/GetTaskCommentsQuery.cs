using MediatR;

namespace Application.Features.Comments.Queries.GetTaskComments
{
    public class GetTaskCommentsQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid TaskId { get; }

        public GetTaskCommentsQuery(Guid taskId)
        {
            TaskId = taskId;
        }
    }
}
