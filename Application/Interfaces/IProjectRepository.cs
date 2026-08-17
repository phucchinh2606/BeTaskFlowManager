using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> GetProjectWithMembersAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
