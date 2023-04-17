import { Todo, TodoItemStatus } from "./todo";

interface TodoItemProps {
  todo: Todo;
  onEdit: (todo: Todo) => void;
  onDelete: (todo: Todo) => void;
}

function TodoItem({ todo, onEdit, onDelete }: TodoItemProps) {
  return (
    <>
      <div className="itemName">
        <div>{todo.name}</div>
      </div>

      <div className="itemPriority">
        <div>p.{todo.priority}</div>
      </div>

      <div>
        <div className={TodoItemStatus[todo.status]}>
          {todo.GetTodoItemStatusString()}
        </div>
      </div>

      <div className="itemEdit">
        <button onClick={() => onEdit(todo)}>
          <span /*className="icon-edit"*/>Edit</span>
        </button>
      </div>
      <div className="itemDelete">
        <button
          onClick={() => onDelete(todo)}
          disabled={todo.status !== TodoItemStatus.Completed}
        >
          <span>Delete</span>
        </button>
      </div>
    </>
  );
}

export default TodoItem;
