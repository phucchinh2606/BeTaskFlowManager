using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries.GetEmployeePerformance
{
    public class GetEmployeePerformanceQuery : IRequest<IEnumerable<EmployeePerformanceDto>>
    {
    }
}
