using Application.Interfaces;
using Application.Interfaces.Authentication;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here
            // Đăng ký DbContext với PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            // Đăng ký Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Đăng ký Task Repository
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            // 1. Đọc chuỗi kết nối Redis từ AppSettings / Env Var, nếu không có mới dùng localhost
            var redisConnectionString = configuration.GetConnectionString("Redis")
                                        ?? "localhost:6379,abortConnect=false";

            // 2. Đăng ký Redis Cache với chuỗi kết nối động
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            return services;
        }
    }
}
