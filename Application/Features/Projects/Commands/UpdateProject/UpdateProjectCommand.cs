using MediatR;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
