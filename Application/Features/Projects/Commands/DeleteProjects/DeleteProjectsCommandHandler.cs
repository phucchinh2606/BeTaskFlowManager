using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Commands.DeleteProjects
{
    public class DeleteProjectsCommandHandler : IRequestHandler<DeleteProjectsCommand, int>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectsCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<int> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Distinct().ToArray();
            if (ids.Length == 0)
            {
                throw new ArgumentException("Cần chọn ít nhất một dự án để xóa.");
            }

            var projects = new List<Domain.Entities.Project>();
            foreach (var id in ids)
            {
                var project = await _projectRepository.GetByIdAsync(id);
                if (project == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy dự án với ID: {id}");
                }

                projects.Add(project);
            }

            await _projectRepository.DeleteRangeAsync(projects, cancellationToken);
            return projects.Count;
        }
    }
}