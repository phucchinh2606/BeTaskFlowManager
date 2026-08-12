using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                throw new Exception($"Không tìm thấy dự án với Id: {request.Id}");
            }

            // Cập nhật thông tin
            project.Name = request.Name;
            project.Description = request.Description;

            await _projectRepository.UpdateAsync(project);
            return true;
        }
    }
}
