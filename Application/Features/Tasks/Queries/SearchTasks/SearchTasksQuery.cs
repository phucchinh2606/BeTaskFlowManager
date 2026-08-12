using Application.Features.Tasks.Queries.GetAllTasks;
using MediatR;

namespace Application.Features.Tasks.Queries.SearchTasks
{
    public class SearchTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public string SearchTerm { get; }

        public SearchTasksQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
