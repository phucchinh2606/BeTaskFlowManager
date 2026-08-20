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
            const string adminEmail = "admin@system.com";
            var adminUser = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == adminEmail.ToLower());

            if (adminUser == null)
            {
                adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    FullName = "System Administrator",
                    SystemRole = SystemRole.Admin,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Users.AddAsync(adminUser);
            }

            adminUser.SystemRole = SystemRole.Admin;
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!@#");
            await context.SaveChangesAsync();
        }
    }
}
