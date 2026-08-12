using Application.Interfaces;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectStatistics
{
    public class GetProjectStatisticsQueryHandler : IRequestHandler<GetProjectStatisticsQuery, ProjectStatisticsDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;

        public GetProjectStatisticsQueryHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<ProjectStatisticsDto> Handle(GetProjectStatisticsQuery request, CancellationToken cancellationToken)
        {
            // 1. Khởi tạo Cache Key duy nhất cho dự án này
            string cacheKey = $"ProjectStats_{request.ProjectId}";

            // 2. Kiểm tra xem Redis đã có dữ liệu chưa
            var cachedStats = await _cacheService.GetAsync<ProjectStatisticsDto>(cacheKey, cancellationToken);
            if (cachedStats != null)
            {
                return cachedStats; // Cache Hit: Trả về ngay lập tức, không chạm vào DB!
            }

            // 3. Cache Miss: Chưa có dữ liệu, tiến hành query xuống Database
            var statsFromDb = await _taskRepository.GetTaskStatisticsByProjectIdAsync(request.ProjectId, cancellationToken);

            var result = new ProjectStatisticsDto
            {
                ProjectId = request.ProjectId,
                StatusCounts = statsFromDb
            };

            // 4. Lưu kết quả vào Redis để dùng cho các lần gọi sau (Cache 5 phút)
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);

            return result;
        }
    }
}
