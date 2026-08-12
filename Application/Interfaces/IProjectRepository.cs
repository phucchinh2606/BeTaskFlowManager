using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        // Có thể bổ sung các hàm query riêng cho Project sau này nếu cần
    }
}
