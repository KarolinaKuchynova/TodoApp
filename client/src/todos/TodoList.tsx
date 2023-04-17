import { useState } from "react";
import { Todo } from "./todo";
import TodoForm from "./TodoEditForm";
import TodoItem from "./TodoItem";

export interface TodoListProps {
  todos: Todo[];
  onSave: (updatedTodo: Todo) => Promise<void>;
  onDelete: (todoToDelete: Todo) => void;
}

function TodoList({ todos, onSave, onDelete }: TodoListProps) {
  const [todoForEdit, setTodoForEdit] = useState({});

  const handleEdit = (todoForEdit: Todo) => {
    setTodoForEdit(todoForEdit);
  };

  const handleEditCancel = () => {
    setTodoForEdit({});
  };

  const handleEditSave = async (updatedTodo: Todo) => {
    await onSave(updatedTodo);
    setTodoForEdit({});
  };

  return (
    <>
      <div>
        {todos.map((todo) => (
          <div key={todo.id} className="todoRow">
            {todoForEdit !== todo ? (
              <TodoItem todo={todo} onEdit={handleEdit} onDelete={onDelete} />
            ) : (
              <TodoForm
                allTodos={todos}
                todoToEdit={todo}
                onCancel={handleEditCancel}
                onSave={handleEditSave}
              />
            )}
          </div>
        ))}
      </div>
    </>
  );
}

export default TodoList;
