using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Comments.Commands.AddComment
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;

        public AddCommentCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository)
        {
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
        }

        public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new ArgumentException("Nội dung bình luận không được để trống.");
            }

            // 1. Kiểm tra Task có tồn tại trong hệ thống không
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Không tìm thấy công việc để thêm bình luận.");
            }

            // 2. Khởi tạo đối tượng Comment
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                UserId = request.UserId,
                Content = request.Content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // 3. Lưu vào CSDL
            await _commentRepository.AddAsync(comment);

            return comment.Id;
        }
    }
}
