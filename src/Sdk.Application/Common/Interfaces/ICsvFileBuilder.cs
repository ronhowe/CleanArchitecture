using Sdk.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Sdk.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
