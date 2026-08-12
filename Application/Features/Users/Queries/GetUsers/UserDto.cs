namespace Application.Features.Users.Queries.GetUsers
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SystemRole { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
