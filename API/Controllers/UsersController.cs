using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetEmployeePerformance;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);

                // Trả về mã 201 Created cùng với Id của user mới
                return CreatedAtAction(nameof(CreateUser), new { id = userId }, new { UserId = userId, Message = "Tạo người dùng thành công" });
            }
            catch (Exception ex)
            {
                // Catch tạm thời để trả lỗi. Nếu có Global Exception Middleware thì không cần khối try-catch này.
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "ID trên URL không khớp với dữ liệu gửi lên." });
            }

            await _mediator.Send(command);
            return Ok(new { Message = "Cập nhật thông tin người dùng thành công." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new { Message = "Xóa người dùng thành công." });
        }

        [HttpGet("performance")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeePerformance()
        {
            var query = new GetEmployeePerformanceQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
