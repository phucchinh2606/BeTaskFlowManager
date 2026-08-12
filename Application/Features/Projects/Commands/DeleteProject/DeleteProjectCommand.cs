using MediatR;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
