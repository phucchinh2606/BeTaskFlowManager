using Application.Interfaces;
using Application.Interfaces.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                throw new Exception("Email hoặc mật khẩu không chính xác.");
            }

            // Kiểm tra mật khẩu băm chuẩn Identity
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Email hoặc mật khẩu không chính xác.");
            }

            // Tạo JWT Token trả về
            return _jwtProvider.Generate(user);
        }
    }
}
