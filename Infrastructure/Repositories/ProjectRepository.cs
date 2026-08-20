using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Project?> GetProjectWithMembersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.ProjectMembers)
                    .ThenInclude(pm => pm.User)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<Project> projects, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(projects);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
