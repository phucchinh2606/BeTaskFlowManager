using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminAsync(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            bool hasAdmin = await context.Users.AnyAsync(u => u.SystemRole == SystemRole.Admin);
            if (!hasAdmin)
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@system.com",
                    FullName = "System Administrator",
                    SystemRole = SystemRole.Admin,
                    CreatedAt = DateTime.UtcNow
                };

                // Băm mật khẩu bằng hasher được truyền vào
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!@#");

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
