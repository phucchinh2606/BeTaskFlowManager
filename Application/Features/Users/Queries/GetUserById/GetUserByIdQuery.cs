using Application.Features.Users.Queries.GetUsers;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
