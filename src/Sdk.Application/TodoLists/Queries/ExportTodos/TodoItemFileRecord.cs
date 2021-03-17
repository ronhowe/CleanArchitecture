using Sdk.Application.Common.Mappings;
using Sdk.Domain.Entities;

namespace Sdk.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
