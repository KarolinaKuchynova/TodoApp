using TodoAppServer.Models;

namespace TodoAppServer.Repositories
{
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Returns todo with requested id or null if no such todo exists.
        /// </summary>
        /// <param name="id">Todo id.</param>
        /// <returns>Todo item or null.</returns>
        TodoItem? GetTodoById(int id);

        /// <summary>
        /// Returns all stored todo items.
        /// </summary>
        IEnumerable<TodoItem> GetAllTodos();

        /// <summary>
        /// Stores given todo.
        /// </summary>
        /// <param name="todo">Todo to insert.</param>
        /// <returns>Inserted todo with new id.</returns>
        TodoItem InsertTodo(TodoItem todo);

        /// <summary>
        /// Updates stored todo or does nothing if the todo does not exist in the database.
        /// </summary>
        /// <param name="todo">Updated todo.</param>
        void UpdateTodo(TodoItem todo);

        /// <summary>
        /// Deletes requested todo or does nothing if no todo with requested id exists.
        /// </summary>
        /// <param name="todoId">Id of the todo to delete.</param>
        void DeleteTodo(int todoId);
    }
}
