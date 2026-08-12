using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.User) // Load thông tin Tác giả (User)
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.CreatedAt) // Mới nhất nằm ở cuối danh sách (Chuẩn hiển thị khung Chat)
                .ToListAsync(cancellationToken);
        }
    }
}
