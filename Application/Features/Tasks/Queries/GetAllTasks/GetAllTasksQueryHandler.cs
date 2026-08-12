using Application.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Queries.GetAllTasks
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            // Gọi qua Repository để lấy dữ liệu đã được map sẵn
            return await _taskRepository.GetAllTasksAsync(cancellationToken);
        }
    }
}
