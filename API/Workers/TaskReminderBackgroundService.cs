using Application.Interfaces;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace API.Workers
{
    public class TaskReminderBackgroundService : BackgroundService
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskReminderBackgroundService> _logger;

        public TaskReminderBackgroundService(
            IHubContext<NotificationHub, INotificationClient> hubContext,
            IServiceScopeFactory scopeFactory,
            ILogger<TaskReminderBackgroundService> logger)
        {
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Reminder Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // 1. Lấy ITaskRepository thay vì AppDbContext
                        var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                        // 2. Gọi hàm từ Repository (Tìm Task đến hạn trong 1 ngày tới)
                        var urgentTasks = await taskRepository.GetAllUrgentTasksAsync(1, stoppingToken);

                        // 3. Gom nhóm theo AssigneeId để gửi thông báo
                        var groupedTasks = urgentTasks.GroupBy(t => t.AssigneeId);

                        foreach (var group in groupedTasks)
                        {
                            var assigneeId = group.Key.ToString();
                            var taskCount = group.Count();

                            // Gửi thông báo đích danh
                            await _hubContext.Clients.User(assigneeId!).ReceiveTaskNotification(
                                "Nhắc nhở công việc khẩn cấp!",
                                $"Bạn có {taskCount} công việc sắp đến hạn hoặc đã quá hạn.",
                                ""
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi xảy ra trong quá trình quét Task khẩn cấp.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
