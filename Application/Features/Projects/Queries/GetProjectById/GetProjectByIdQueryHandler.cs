using Application.Features.Projects.Queries.GetAllProjects;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                throw new Exception($"Không tìm thấy dự án với ID: {request.Id}");
            }

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                IsArchived = project.IsArchived,
                CreatedById = project.CreatedById,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
