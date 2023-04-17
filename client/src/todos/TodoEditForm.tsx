import { SyntheticEvent, useState } from "react";
import { Todo, TodoItemStatus } from "./todo";

interface TodoEditFormProps {
  allTodos: Todo[];
  todoToEdit: Todo;
  onCancel: () => void;
  onSave: (updatedTodo: Todo) => void;
}

function TodoEditForm({
  allTodos,
  todoToEdit,
  onCancel,
  onSave,
}: TodoEditFormProps) {
  const [errors, setErrors] = useState({ name: "", priority: "" });
  const [nameText, setNameText] = useState(todoToEdit.name);
  const [priorityText, setPriorityText] = useState(
    todoToEdit.priority.toString()
  );
  const [selectedStatus, setSelectedStatus] = useState(todoToEdit.status);

  const handleSubmit = (event: SyntheticEvent) => {
    event.preventDefault();
    if (!isValid()) return;
    onSave(
      new Todo({
        name: nameText,
        priority: Number(priorityText),
        status: selectedStatus,
        id: todoToEdit.id,
      })
    );
  };

  const isValid = () => {
    return errors.name.length === 0 && errors.priority.length === 0;
  };

  const handleNameChange = (event: any) => {
    const newName = event.target.value;
    setNameText(newName);
    let nameError = { name: "" };
    if (newName.length === 0) {
      nameError.name = "Name is required.";
    } else if (
      allTodos.some((x) => x.name === newName && x.id !== todoToEdit.id)
    ) {
      nameError.name = "Task with this name already exists.";
    }

    let updatedErrors: { name: string; priority: string };
    setErrors((er) => {
      updatedErrors = { ...er, ...nameError };
      return updatedErrors;
    });
  };

  const handlePriorityChange = (event: any) => {
    const newPriority = event.target.value;
    setPriorityText(newPriority);
    let priorityError = { priority: "" };
    if (isNaN(Number(newPriority))) {
      priorityError.priority = "Priority must be a valid number.";
    }

    let updatedErrors: { name: string; priority: string };
    setErrors((er) => {
      updatedErrors = { ...er, ...priorityError };
      return updatedErrors;
    });
  };

  return (
    <form className="editForm" onSubmit={handleSubmit}>
      {/* <label htmlFor="name">Task</label> */}
      <div className="itemEditName">
        <input
          type="text"
          name="name"
          placeholder="enter name"
          value={nameText}
          onChange={handleNameChange}
        />
      </div>

      {/* <label htmlFor="priority">Priority</label> */}
      <div className="itemEditPriority">
        <input
          type="text"
          name="priority"
          placeholder="enter priority"
          value={priorityText}
          onChange={handlePriorityChange}
        />
      </div>

      {/* <label htmlFor="status">Status</label> */}
      <div className="itemEditStatus">
        <select
          name="status"
          value={selectedStatus}
          onChange={(e) =>
            setSelectedStatus(
              TodoItemStatus[e.target.value as keyof typeof TodoItemStatus]
            )
          }
        >
          <option value={TodoItemStatus[TodoItemStatus.NotStarted]}>
            Not Started
          </option>
          <option value={TodoItemStatus[TodoItemStatus.InProgress]}>
            In Progress
          </option>
          <option value={TodoItemStatus[TodoItemStatus.Completed]}>
            Completed
          </option>
        </select>
      </div>

      <div className="itemEditSave">
        <button>Save</button>
      </div>

      <div className="itemEditCancel">
        <button type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>

      {errors.name.length > 0 && (
        <div className="editError">
          <p>{errors.name}</p>
        </div>
      )}

      {errors.priority.length > 0 && (
        <div className="editError">
          <p>{errors.priority}</p>
        </div>
      )}
    </form>
  );
}

export default TodoEditForm;
