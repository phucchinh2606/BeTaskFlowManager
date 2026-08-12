using Application.Features.Projects.Commands.ArchiveProject;
using Application.Features.Projects.Commands.CreateProject;
using Application.Features.Projects.Commands.DeleteProject;
using Application.Features.Projects.Commands.UpdateProject;
using Application.Features.Projects.Queries.GetAllProjects;
using Application.Features.Projects.Queries.GetProjectById;
using Application.Features.Projects.Queries.GetProjectStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            var projectId = await _mediator.Send(command);

            // Trả về mã 201 Created cùng với Id của project mới
            return CreatedAtAction(nameof(CreateProject), new { id = projectId }, new { ProjectId = projectId, Message = "Tạo dự án thành công" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "Id trên URL không khớp với dữ liệu gửi lên." });
            }

            await _mediator.Send(command);
            return Ok(new { Message = "Cập nhật thông tin dự án thành công." });
        }

        [HttpPut("{id}/archive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveProject(Guid id)
        {
            var command = new ArchiveProjectCommand { Id = id };
            await _mediator.Send(command);

            return Ok(new { Message = "Đã lưu trữ (archive) dự án thành công." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects()
        {
            var query = new GetAllProjectsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var query = new GetProjectByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var command = new DeleteProjectCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new { Message = "Xóa dự án thành công." });
        }

        [HttpGet("{id}/statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectStatistics(Guid id)
        {
            var query = new GetProjectStatisticsQuery { ProjectId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
