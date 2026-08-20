using Application.Features.Comments.Commands.AddComment;
using Application.Features.Comments.Queries.GetTaskComments;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Features.Tasks.Commands.DeleteTask;
using Application.Features.Tasks.Commands.UpdateTask;
using Application.Features.Tasks.Commands.UpdateTaskStatus;
using Application.Features.Tasks.Queries.FilterTasks;
using Application.Features.Tasks.Queries.GetAllTasks;
using Application.Features.Tasks.Queries.GetMyTasks;
using Application.Features.Tasks.Queries.GetTaskById;
using Application.Features.Tasks.Queries.GetUrgentTasks;
using Application.Features.Tasks.Queries.SearchTasks;
using Domain.Enums;
using Infrastructure.SignalR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;

        public TasksController(IMediator mediator, IHubContext<NotificationHub, INotificationClient> notificationHub)
        {
            _mediator = mediator;
            _notificationHub = notificationHub;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            var taskId = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateTask), new { id = taskId }, new { TaskId = taskId, Message = "Tạo công việc thành công." });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
        {
            // Đảm bảo ID trên URL khớp với ID trong Body để tránh cập nhật nhầm
            if (id != command.Id)
            {
                return BadRequest(new { Message = "ID trên URL không khớp với dữ liệu gửi lên." });
            }

            try
            {
                await _mediator.Send(command);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }

            return Ok(new { Message = "Cập nhật công việc thành công." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            var query = new GetAllTasksQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var query = new GetTaskByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var command = new DeleteTaskCommand { Id = id };
            try
            {
                await _mediator.Send(command);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }

            return Ok(new { Message = "Xóa công việc thành công." });
        }

        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            // 1. Lấy UserId từ Claims của JWT Token đã đăng nhập
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized(new { Message = "Không xác thực được người dùng." });
            }

            // 2. Gửi Query chứa CurrentUserId xử lý
            var query = new GetMyTasksQuery(currentUserId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("urgent-tasks")]
        public async Task<IActionResult> GetUrgentTasks([FromQuery] int daysThreshold = 3)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized(new { Message = "Không xác thực được người dùng." });
            }

            var query = new GetUrgentTasksQuery(currentUserId, daysThreshold);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusRequest request)
        {
            if (!Enum.TryParse<Domain.Enums.TaskStatus>(request.Status, ignoreCase: true, out var status) ||
                !Enum.IsDefined(status))
            {
                return BadRequest(new
                {
                    Message = "Trạng thái không hợp lệ. Giá trị hợp lệ: ToDo, InProgress, Review, Done."
                });
            }

            // Lấy UserId từ Claims của JWT Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized(new { Message = "Không xác thực được người dùng." });
            }

            var command = new UpdateTaskStatusCommand(id, status, currentUserId);

            try
            {
                await _mediator.Send(command);
                return Ok(new { Message = "Cập nhật tiến độ công việc thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
            }
        }

        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment(Guid taskId, [FromBody] AddCommentRequest request)
        {
            // Lấy UserId từ Token JWT của người dùng hiện tại
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized(new { Message = "Không xác thực được người dùng." });
            }

            var command = new AddCommentCommand(taskId, currentUserId, request.Content);

            try
            {
                var commentId = await _mediator.Send(command);
                var authorEmail = User.FindFirst(ClaimTypes.Email)?.Value
                    ?? User.FindFirst("email")?.Value
                    ?? "Người dùng";
                await _notificationHub.Clients.Group(NotificationHub.TaskGroup(taskId)).ReceiveComment(
                    new CommentNotification(commentId, taskId, currentUserId, request.Content.Trim(), authorEmail, DateTime.UtcNow));
                return CreatedAtAction(nameof(AddComment), new { taskId, commentId }, new { CommentId = commentId, Message = "Thêm bình luận thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetTaskComments(Guid taskId)
        {
            var query = new GetTaskCommentsQuery(taskId);

            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string title)
        {
            var query = new SearchTasksQuery(title);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterTasks(
            [FromQuery] Domain.Enums.TaskStatus? status,
            [FromQuery] Guid? assigneeId,
            [FromQuery] PriorityLevel? priority)
        {
            var query = new FilterTasksQuery(status, assigneeId, priority);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
