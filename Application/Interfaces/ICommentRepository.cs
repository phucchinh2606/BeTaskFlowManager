using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    }
}
