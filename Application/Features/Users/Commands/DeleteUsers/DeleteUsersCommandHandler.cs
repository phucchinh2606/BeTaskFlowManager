using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUsers
{
    public class DeleteUsersCommandHandler : IRequestHandler<DeleteUsersCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUsersCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Distinct().ToArray();
            if (ids.Length == 0)
            {
                throw new ArgumentException("Cần chọn ít nhất một thành viên để xóa.");
            }

            var users = new List<Domain.Entities.User>();
            foreach (var id in ids)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy thành viên với ID: {id}");
                }

                users.Add(user);
            }

            await _userRepository.DeleteRangeAsync(users, cancellationToken);
            return users.Count;
        }
    }
}
