namespace Application.Features.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusRequest
    {
        // Nhận chuỗi từ JSON để tránh Model Binding trả 400 khi client gửi
        // "ToDo", "InProgress", "Review" hoặc "Done".
        public string Status { get; set; } = string.Empty;
    }
}
