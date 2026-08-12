using Domain.Entities;

namespace Application.Interfaces.Authentication
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
