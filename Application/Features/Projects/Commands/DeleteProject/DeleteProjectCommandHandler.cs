using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                throw new Exception($"Không tìm thấy dự án với ID: {request.Id}");
            }

            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}
