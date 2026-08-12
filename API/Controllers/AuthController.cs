using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var token = await _mediator.Send(command);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // Trả về HTTP Status 400 (Bad Request) kèm thông báo lỗi thay vì văng app
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize] // Yêu cầu phải có Token hợp lệ mới được gọi API này
        public async Task<IActionResult> Logout()
        {
            // Lấy UserId từ Claim 'sub' đã được mã hóa trong JwtProvider
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "Không xác thực được người dùng." });
            }

            var command = new LogoutCommand(userId);
            await _mediator.Send(command);

            return Ok(new { Message = "Đăng xuất thành công." });
        }


    }
}
