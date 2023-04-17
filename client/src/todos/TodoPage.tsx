import TodoList from "./TodoList";
import { Todo } from "./todo";
import { useState, useEffect } from "react";
import { todoAPI } from "./todoAPI";
import TodoAddForm from "./TodoAddForm";

function TodoPage() {
  const [loading, setLoading] = useState<Boolean>(false);
  const [error, setError] = useState<string | undefined>(undefined);
  const [todos, setTodos] = useState<Todo[]>([]);

  const saveUpdatedTodo = async (updatedTodo: Todo) => {
    try {
      await todoAPI.put(updatedTodo);
      let updatedTodos = todos.map((t: Todo) => {
        return t.id === updatedTodo.id ? updatedTodo : t;
      });
      setTodos(updatedTodos);
    } catch (e) {
      if (e instanceof Error) {
        setError(e.message);
      }
    }
  };

  const deleteTodo = async (todoToDelete: Todo) => {
    try {
      await todoAPI.delete(todoToDelete);
      let updatedTodos = todos.filter((t: Todo) => t.id !== todoToDelete.id);
      setTodos(updatedTodos);
    } catch (e) {
      if (e instanceof Error) {
        setError(e.message);
      }
    }
  };

  const addTodo = async (newTodo: Todo) => {
    try {
      const addedTodo = await todoAPI.post(newTodo);
      let updatedTodos = todos.slice();
      updatedTodos.push(addedTodo);
      setTodos(updatedTodos);
    } catch (e) {
      if (e instanceof Error) {
        setError(e.message);
      }
    }
  };

  useEffect(() => {
    async function loadTodos() {
      setLoading(true);
      try {
        const data = await todoAPI.get();
        setError("");
        setTodos(data);
      } catch (e) {
        if (e instanceof Error) {
          setError(e.message);
        }
      } finally {
        setLoading(false);
      }
    }
    loadTodos();
  }, []);

  return (
    <>
      <h1 className="pageHeader">My Todo List</h1>
      {error && (
        <div className="row">
          <div className="pageError">
            <section>
              <p>
                <span></span>
                {error}
              </p>
            </section>
          </div>
        </div>
      )}

      <TodoAddForm existingTodos={todos} onAdd={addTodo} />
      <TodoList todos={todos} onSave={saveUpdatedTodo} onDelete={deleteTodo} />

      {loading && (
        <div className="center-page">
          <span className="spinner primary"></span>
          <p>Loading...</p>
        </div>
      )}
    </>
  );
}

export default TodoPage;
