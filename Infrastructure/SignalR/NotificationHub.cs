using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Infrastructure.SignalR
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        public static string TaskGroup(Guid taskId) => $"task:{taskId}";

        public Task JoinTask(string taskId)
        {
            if (!Guid.TryParse(taskId, out var parsedTaskId))
            {
                throw new HubException("Task ID không hợp lệ.");
            }

            return Groups.AddToGroupAsync(Context.ConnectionId, TaskGroup(parsedTaskId));
        }

        public Task LeaveTask(string taskId) => Guid.TryParse(taskId, out var parsedTaskId)
            ? Groups.RemoveFromGroupAsync(Context.ConnectionId, TaskGroup(parsedTaskId))
            : Task.CompletedTask;

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Context.User?.FindFirst("sub")?.Value;
            Console.WriteLine($"User {userId} connected to NotificationHub.");
            await base.OnConnectedAsync();
        }
    }
}
