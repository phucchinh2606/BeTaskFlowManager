namespace Application.Features.Comments.Queries.GetTaskComments
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string AuthorEmail { get; set; } = string.Empty;
    }
}
