using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public ArchiveProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                throw new Exception($"Không tìm thấy dự án với Id: {request.Id}");
            }

            // Đổi trạng thái sang Lưu trữ
            project.IsArchived = true;

            await _projectRepository.UpdateAsync(project);
            return true;
        }
    }
}
