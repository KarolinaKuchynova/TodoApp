using Microsoft.AspNetCore.Mvc;
using TodoAppServer.Models;
using TodoAppServer.Repositories;

namespace TodoAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemRepository _todoRepository;

        public TodoItemsController(ITodoItemRepository todoRepository) {
            _todoRepository = todoRepository;
        }

        // GET: api/TodoItems
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems() {
            return _todoRepository.GetAllTodos();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(int id) {
            var todoItem = _todoRepository.GetTodoById(id);

            if (todoItem == null) {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem([FromBody] TodoItem todoItem) {
            RunTodoListValidations(todoItem);

            if (!ModelState.IsValid) {
                return UnprocessableEntity(ModelState);
            }

            var insertedTodoItem = _todoRepository.InsertTodo(todoItem);

            return insertedTodoItem;
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public IActionResult PutTodoItem(int id, [FromBody] TodoItem todoItem) {
            if (id != todoItem.Id) {
                return BadRequest();
            }

            RunTodoListValidations(todoItem);

            if (!ModelState.IsValid) {
                return UnprocessableEntity(ModelState);
            }

            _todoRepository.UpdateTodo(todoItem);

            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id) {
            var todoItem = _todoRepository.GetTodoById(id);
            if (todoItem == null) {
                return NotFound();
            }

            _todoRepository.DeleteTodo(id);

            return NoContent();
        }

        private void RunTodoListValidations(TodoItem changedTodoItem) {
            var allTodos = _todoRepository.GetAllTodos();
            if (IsTodoItemNameDuplicate(changedTodoItem, allTodos)) {
                ModelState.AddModelError(nameof(TodoItem.Name), "Tasks with this name already exists.");
            }
            return;
        }

        private static bool IsTodoItemNameDuplicate(TodoItem todoToCheck, IEnumerable<TodoItem> existingTodoItems) {
            return existingTodoItems.Any(x => x.Name == todoToCheck.Name && x.Id != todoToCheck.Id);
        }
    }
}
