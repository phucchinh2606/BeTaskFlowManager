using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetEmployeePerformance
{
    public class GetEmployeePerformanceQueryHandler : IRequestHandler<GetEmployeePerformanceQuery, IEnumerable<EmployeePerformanceDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetEmployeePerformanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<EmployeePerformanceDto>> Handle(GetEmployeePerformanceQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetEmployeePerformanceAsync(cancellationToken);
        }
    }
}
