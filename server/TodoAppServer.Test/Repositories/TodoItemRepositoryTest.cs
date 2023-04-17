using NUnit.Framework;
using TodoAppServer.Models;
using TodoAppServer.Repositories;

namespace TodoAppServer.Test.Repositories
{
    internal class TodoItemRepositoryTest
    {
        [Test]
        public void TestGetTodoByIdReturnsCorrectTodoItem() {
            var testObj = new TodoItemRepository();
            testObj.InsertTodo(new TodoItem());
            testObj.InsertTodo(new TodoItem());
            var requestedTodo = new TodoItem();
            testObj.InsertTodo(requestedTodo);

            var returnedTodo = testObj.GetTodoById(requestedTodo.Id);

            Assert.AreEqual(requestedTodo, returnedTodo);
        }

        [Test]
        public void TestGetTodoByIdReturnsNullForNonexistentId() {
            var testObj = new TodoItemRepository();
            testObj.InsertTodo(new TodoItem());
            testObj.InsertTodo(new TodoItem());

            int nonexistentId = 100;
            var returnedTodo = testObj.GetTodoById(nonexistentId);

            Assert.IsNull(returnedTodo);
        }

        [Test]
        public void TestGetAllTodosReturnsAllInsertedTodos() {
            var testObj = new TodoItemRepository();
            var firstTodo = new TodoItem();
            testObj.InsertTodo(firstTodo);
            var secondTodo = new TodoItem();
            testObj.InsertTodo(secondTodo);
            var thirdTodo = new TodoItem();
            testObj.InsertTodo(thirdTodo);

            var returnedTodos = testObj.GetAllTodos();

            Assert.NotNull(returnedTodos);
            var returnedTodosList = returnedTodos.ToList();
            Assert.AreEqual(3, returnedTodosList.Count());
            Assert.Contains(firstTodo, returnedTodosList);
            Assert.Contains(secondTodo, returnedTodosList);
            Assert.Contains(thirdTodo, returnedTodosList);
        }

        [Test]
        public void TestGetAllTodosReturnsEmptyListIfNoTodosWereInserted() {
            var testObj = new TodoItemRepository();

            var returnedTodos = testObj.GetAllTodos();

            Assert.NotNull(returnedTodos);
            Assert.IsEmpty(returnedTodos);
        }

        [Test]
        public void TestInsertTodoAddsNewTodo() {
            var testObj = new TodoItemRepository();

            var todoForInsert = new TodoItem();
            testObj.InsertTodo(todoForInsert);

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(1, allTodos.Count);
            Assert.AreEqual(todoForInsert, allTodos[0]);
            Assert.AreEqual(1, allTodos[0].Id);
        }

        [Test]
        public void TestInsertTodoAddsMultipleTodos() {
            var testObj = new TodoItemRepository();
            var firstTodoForInsert = new TodoItem();
            var secondTodoForInsert = new TodoItem();
            var thirdTodoForInsert = new TodoItem();

            testObj.InsertTodo(firstTodoForInsert);
            testObj.InsertTodo(secondTodoForInsert);
            testObj.InsertTodo(thirdTodoForInsert);

            firstTodoForInsert.Id = 1;
            secondTodoForInsert.Id = 2;
            thirdTodoForInsert.Id = 3;

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(3, allTodos.Count);
            Assert.Contains(firstTodoForInsert, allTodos);
            Assert.Contains(secondTodoForInsert, allTodos);
            Assert.Contains(thirdTodoForInsert, allTodos);
        }

        [Test]
        public void TestUpdateTodoUpdatesInsertedTodo() {
            var testObj = new TodoItemRepository();
            var insertedTodo = new TodoItem() { Name = "Old name" };
            testObj.InsertTodo(insertedTodo);

            var updatedTodo = new TodoItem() { Id = insertedTodo.Id, Name = "New name" };
            testObj.UpdateTodo(updatedTodo);

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(1, allTodos.Count);
            Assert.Contains(updatedTodo, allTodos);
        }

        [Test]
        public void TestUpdateTodoDoesNothingForNonexistentTodo() {
            var testObj = new TodoItemRepository();
            var insertedTodo = new TodoItem();
            testObj.InsertTodo(insertedTodo);

            var newTodo = new TodoItem() { Id = 100 };
            testObj.UpdateTodo(newTodo);

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(1, allTodos.Count);
            Assert.Contains(insertedTodo, allTodos);
        }

        [Test]
        public void TestDeleteTodoDeletesSelectedTodo() {
            var testObj = new TodoItemRepository();
            var firstTodo = new TodoItem();
            var secondTodo = new TodoItem();
            testObj.InsertTodo(firstTodo);
            testObj.InsertTodo(secondTodo);

            testObj.DeleteTodo(firstTodo.Id);

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(1, allTodos.Count);
            Assert.Contains(secondTodo, allTodos);
        }

        [Test]
        public void TestDeleteTodoDoesNothingForNonexistentTodoId() {
            var testObj = new TodoItemRepository();
            var insertedTodo = new TodoItem();
            testObj.InsertTodo(insertedTodo);

            var nonexistentTodoId = 100;
            testObj.DeleteTodo(nonexistentTodoId);

            var allTodos = testObj.GetAllTodos().ToList();
            Assert.AreEqual(1, allTodos.Count);
            Assert.Contains(insertedTodo, allTodos);
        }
    }
}
