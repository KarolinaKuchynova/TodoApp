import { SyntheticEvent, useState } from "react";
import { Todo, TodoItemStatus } from "./todo";

interface TodoAddFormProps {
  existingTodos: Todo[];
  onAdd: (newTodo: Todo) => Promise<void>;
}

function TodoAddForm({ existingTodos, onAdd }: TodoAddFormProps) {
  const [newTodoText, setNewTodoText] = useState("");
  const [saveNewTodoError, setSaveNewTodoError] = useState("");

  function handleNewTaskNameChange(event: any) {
    const newName = event.target.value;
    setNewTodoText(newName);

    if (existingTodos.some((x) => x.name === newName)) {
      setSaveNewTodoError("Task with this name already exists.");
    } else {
      setSaveNewTodoError("");
    }
  }

  const handleAdd = async (event: SyntheticEvent) => {
    event.preventDefault();
    if (saveNewTodoError.length > 0) return;
    if (newTodoText.length === 0) {
      setSaveNewTodoError("Task name is required.");
      return;
    }

    const newTodo = new Todo({
      name: newTodoText,
      priority: 1,
      status: TodoItemStatus.NotStarted,
    });

    await onAdd(newTodo);
    setNewTodoText("");
  };

  return (
    <form className="addForm" onSubmit={handleAdd}>
      <input
        type="text"
        name="name"
        placeholder="Add new task"
        value={newTodoText}
        onChange={handleNewTaskNameChange}
      />
      <button className="primary medium addButton">Add</button>
      {saveNewTodoError.length > 0 && (
        <div className="addError">
          <p>{saveNewTodoError}</p>
        </div>
      )}
    </form>
  );
}

export default TodoAddForm;
