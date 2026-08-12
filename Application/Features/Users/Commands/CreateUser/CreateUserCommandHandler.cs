using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher; // 2. Đổi thành IPasswordHasher<User> của Identity

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Email Unique
            bool isUnique = await _userRepository.IsEmailUniqueAsync(request.Email);
            if (!isUnique)
            {
                throw new Exception($"Email '{request.Email}' đã tồn tại trong hệ thống.");
            }

            // 2. Khởi tạo User Entity
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                SystemRole = SystemRole.User,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Hash Password theo chuẩn Microsoft Identity (truyền cả user và password)
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            // 4. Save to Database
            await _userRepository.AddAsync(user);

            // 5. Trả về Id của user vừa tạo
            return user.Id;
        }
    }
}
