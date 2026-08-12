using Application.Features.Tasks.Queries.GetAllTasks;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Tasks.Queries.FilterTasks
{
    public class FilterTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public Domain.Enums.TaskStatus? Status { get; }
        public Guid? AssigneeId { get; }
        public PriorityLevel? Priority { get; }

        public FilterTasksQuery(Domain.Enums.TaskStatus? status, Guid? assigneeId, PriorityLevel? priority)
        {
            Status = status;
            AssigneeId = assigneeId;
            Priority = priority;
        }
    }
}
