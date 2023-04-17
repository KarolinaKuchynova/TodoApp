using TodoAppServer.Models;

namespace TodoAppServer.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private int _nextId = 1;
        private Dictionary<int, TodoItem> _todos = new Dictionary<int, TodoItem>();

        /// <inheritdoc />
        public TodoItem? GetTodoById(int id) {
            _todos.TryGetValue(id, out TodoItem? item);
            return item;
        }

        /// <inheritdoc />
        public IEnumerable<TodoItem> GetAllTodos() {
            return _todos.Values.ToList();
        }

        /// <inheritdoc />
        public TodoItem InsertTodo(TodoItem item) {
            item.Id = _nextId;
            _todos.Add(item.Id, item);
            _nextId++;

            return item;
        }

        // <inheritdoc />
        public void UpdateTodo(TodoItem item) {
            if (_todos.ContainsKey(item.Id)) {
                _todos[item.Id] = item;
            }
        }

        // <inheritdoc />
        public void DeleteTodo(int todoId) {
            _todos.Remove(todoId);
        }
    }
}
