using Application.Features.Tasks.Queries.GetAllTasks;
using MediatR;

namespace Application.Features.Tasks.Queries.GetUrgentTasks
{
    public class GetUrgentTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Guid UserId { get; set; }
        public int DaysThreshold { get; set; }

        public GetUrgentTasksQuery(Guid userId, int daysThreshold = 3) // Mặc định báo trước 3 ngày
        {
            UserId = userId;
            DaysThreshold = daysThreshold;
        }
    }
}
