using Application.Interfaces;
using MediatR;

namespace Application.Features.Comments.Queries.GetTaskComments
{
    public class GetTaskCommentsQueryHandler : IRequestHandler<GetTaskCommentsQuery, IEnumerable<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITaskRepository _taskRepository;

        public GetTaskCommentsQueryHandler(ICommentRepository commentRepository, ITaskRepository taskRepository)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra Task có tồn tại hay không
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Không tìm thấy công việc.");
            }

            // 2. Lấy danh sách bình luận kèm thông tin User
            var comments = await _commentRepository.GetCommentsByTaskIdAsync(request.TaskId, cancellationToken);

            // 3. Mapping sang DTO
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                AuthorEmail = c.User?.Email ?? string.Empty
            });
        }
    }
}
