using Application.Interfaces;

namespace Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Sử dụng BCrypt để băm mật khẩu với Salt tự động
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
