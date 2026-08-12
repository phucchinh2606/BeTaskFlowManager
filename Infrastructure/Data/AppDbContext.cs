using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique(); // Đảm bảo Email không trùng
                entity.Property(e => e.SystemRole).HasConversion<string>(); // Lưu Enum dạng Text
            });

            // 2. Cấu hình bảng Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            });

            // 3. Cấu hình bảng trung gian ProjectMember (Composite Key)
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => new { pm.ProjectId, pm.UserId }); // Khóa chính kép

                entity.Property(e => e.ProjectRole).HasConversion<string>(); // Lưu Enum dạng Text

                entity.HasOne(pm => pm.Project)
                      .WithMany(p => p.ProjectMembers)
                      .HasForeignKey(pm => pm.ProjectId);

                entity.HasOne(pm => pm.User)
                      .WithMany(u => u.ProjectMembers)
                      .HasForeignKey(pm => pm.UserId);
            });

            // 4. Cấu hình bảng TaskItem
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.Priority).HasConversion<string>();

                // Quan hệ với Project
                entity.HasOne(t => t.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade); // Xóa Project -> xóa luôn Task

                // Quan hệ với User (Assignee)
                entity.HasOne(t => t.Assignee)
                      .WithMany(u => u.AssignedTasks)
                      .HasForeignKey(t => t.AssigneeId)
                      .OnDelete(DeleteBehavior.SetNull); // Xóa User -> Task bị gỡ người phụ trách
            });

            // 5. Cấu hình bảng Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(c => c.Task)
                      .WithMany(t => t.Comments)
                      .HasForeignKey(c => c.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Không cho xóa User nếu họ có comment
            });
        }
    }
}
