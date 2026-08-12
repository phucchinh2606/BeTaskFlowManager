using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            // 1. Khởi tạo đối tượng Project dựa trên entity bạn cung cấp
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                CreatedById = request.CreatedById,
                CreatedAt = DateTime.UtcNow
            };

            // 2. Thêm người tạo vào bảng trung gian ProjectMember với vai trò Manager
            var projectMember = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = request.CreatedById,
                ProjectRole = ProjectRole.Manager, // Vai trò quản lý dự án
                JoinedAt = DateTime.UtcNow
            };

            project.ProjectMembers.Add(projectMember);

            // 3. Lưu xuống Database thông qua Repository
            await _projectRepository.AddAsync(project);

            // 4. Trả về Id của dự án vừa được tạo thành công
            return project.Id;
        }
    }
}
