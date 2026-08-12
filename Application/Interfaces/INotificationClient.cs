namespace Application.Interfaces
{
    public sealed record CommentNotification(
        Guid Id,
        Guid TaskId,
        Guid UserId,
        string Content,
        string AuthorEmail,
        DateTime CreatedAt);

    public interface INotificationClient
    {
        Task ReceiveTaskNotification(string title, string message, string taskId);
        Task ReceiveComment(CommentNotification comment);
    }
}
