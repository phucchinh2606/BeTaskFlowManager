using MediatR;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CreatedById { get; set; } // Id của Manager/User tạo dự án
    }
}
