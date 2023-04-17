using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoAppServer.Controllers;
using TodoAppServer.Models;
using TodoAppServer.Repositories;

namespace TodoAppServer.Test.Controllers
{
    public class TodoItemsControllerTest
    {
        [Test]
        public void TestGetTodoItemsReturnsAllTodosInRepository() {
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            var firstTodo = new TodoItem() { Id = 1 };
            var secondTodo = new TodoItem() { Id = 2 };
            todoRepositoryMock.Setup(x => x.GetAllTodos()).Returns(new[] { firstTodo, secondTodo });
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var returnedTodos = testObj.GetTodoItems().ToList();

            Assert.NotNull(returnedTodos);
            Assert.AreEqual(2, returnedTodos.Count());
            Assert.Contains(firstTodo, returnedTodos);
            Assert.Contains(secondTodo, returnedTodos);

            todoRepositoryMock.Verify(p => p.GetAllTodos(), Times.Once());
        }

        [Test]
        public void TestGetTodoItemReturnsRequestedTodo() {
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            var storedTodoItem = new TodoItem() { Id = 1 };
            todoRepositoryMock.Setup(x => x.GetTodoById(storedTodoItem.Id)).Returns(storedTodoItem);
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var returnedTodo = testObj.GetTodoItem(storedTodoItem.Id);

            Assert.NotNull(returnedTodo.Value);
            Assert.AreEqual(storedTodoItem, returnedTodo.Value);

            todoRepositoryMock.Verify(p => p.GetTodoById(storedTodoItem.Id), Times.Once());
        }
                
        [Test]
        public void TestGetTodoItemReturnsNotFoundWhenRequestedTodoDoesNotExist() {
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(x => x.GetTodoById(It.IsAny<int>())).Returns<TodoItem>(null);
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var nonexistentTodoId = 100;
            var result = testObj.GetTodoItem(nonexistentTodoId);

            Assert.Null(result.Value);
            Assert.NotNull(result.Result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);

            todoRepositoryMock.Verify(p => p.GetTodoById(nonexistentTodoId), Times.Once());
        }
        
        [Test]
        public void TestPostTodoItemInsertsTodoToRepository() {
            var storedTodo = new TodoItem() { Id = 10, Name="Stored item name" };
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(p => p.GetAllTodos()).Returns(new List<TodoItem>() { storedTodo });
            var todoReturnedOnInsert = new TodoItem() { Id = 20};
            todoRepositoryMock.Setup(p => p.InsertTodo(It.IsAny<TodoItem>())).Returns(todoReturnedOnInsert);
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var newTodo = new TodoItem() { Name = "New name" };
            var result = testObj.PostTodoItem(newTodo);

            Assert.NotNull(result.Value);
            Assert.AreEqual(todoReturnedOnInsert, result.Value);

            todoRepositoryMock.Verify(p => p.GetAllTodos(), Times.Once);
            todoRepositoryMock.Verify(p => p.InsertTodo(newTodo), Times.Once());
        }

        [Test]
        public void TestPostTodoItemReturnsUnprocessableEntityWhenTodoNameIsDuplicate() {
            var storedTodo = new TodoItem() { Id = 10, Name = "Taken name" };
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(p => p.GetAllTodos()).Returns(new List<TodoItem>() { storedTodo });
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var newTodo = new TodoItem() {  Name = storedTodo.Name };
            var result = testObj.PostTodoItem(newTodo);

            Assert.Null(result.Value);
            Assert.NotNull(result.Result);
            Assert.IsInstanceOf<UnprocessableEntityObjectResult>(result.Result);

            todoRepositoryMock.Verify(p => p.GetAllTodos(), Times.Once);
        }

        [Test]
        public void TestPutTodoItemWithWrongIdReturnsBadRequest() {
            int todoId = 10;
            var todoItem = new TodoItem() { Id = todoId };
            var testObj = new TodoItemsController(new Mock<ITodoItemRepository>(MockBehavior.Strict).Object);

            int unmatchingRequestId = 1;
            var result = testObj.PutTodoItem(unmatchingRequestId, todoItem);

            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
        }

        [Test]
        public void TestPutTodoItemWithValidItemUpdatesRepository() {
            var storedTodo = new TodoItem() { Id = 10 };
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(p => p.GetAllTodos()).Returns(new List<TodoItem>() { storedTodo });
            todoRepositoryMock.Setup(p => p.UpdateTodo(It.IsAny<TodoItem>()));
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var updatedTodo = new TodoItem() { Id = storedTodo.Id, Name = "New name" };
            var result = testObj.PutTodoItem(storedTodo.Id, updatedTodo);

            var badRequestResult = result as NoContentResult;
            Assert.IsNotNull(badRequestResult);

            todoRepositoryMock.Verify(p => p.GetAllTodos(), Times.Once);
            todoRepositoryMock.Verify(p => p.UpdateTodo(updatedTodo), Times.Once());
        }

        [Test]
        public void TestPutTodoItemReturnsUnprocessableEntityWhenTodoNameIsDuplicate() {
            var storedTodo = new TodoItem() { Id = 10 };
            var otherStoredTodo = new TodoItem() { Id = 20, Name = "Taken name" };
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(p => p.GetAllTodos()).Returns(new List<TodoItem>() { storedTodo, otherStoredTodo });
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            var updatedTodo = new TodoItem() { Id = storedTodo.Id, Name = otherStoredTodo.Name };
            var result = testObj.PutTodoItem(storedTodo.Id, updatedTodo);

            var unprocessableEntityResult = result as UnprocessableEntityObjectResult;
            Assert.IsNotNull(unprocessableEntityResult);

            todoRepositoryMock.Verify(p => p.GetAllTodos(), Times.Once);
        }

        [Test]
        public void TestDeleteTodoWithNotExistentTodoId() {
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(x => x.GetTodoById(It.IsAny<int>())).Returns<TodoItem>(null);
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            int nonExistentItemId = 100;
            var result = testObj.DeleteTodoItem(nonExistentItemId);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);

            todoRepositoryMock.Verify(p => p.GetTodoById(nonExistentItemId), Times.Once());
        }

        [Test]
        public void TestDeleteTodoDeletesTodoInRepository() {
            var todoRepositoryMock = new Mock<ITodoItemRepository>(MockBehavior.Strict);
            todoRepositoryMock.Setup(p => p.GetTodoById(It.IsAny<int>())).Returns(new TodoItem());
            todoRepositoryMock.Setup(x => x.DeleteTodo(It.IsAny<int>()));
            var testObj = new TodoItemsController(todoRepositoryMock.Object);

            int requestedTodoId = 10;
            var result = testObj.DeleteTodoItem(requestedTodoId);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult); 

            todoRepositoryMock.Verify(p => p.GetTodoById(requestedTodoId), Times.Once());
            todoRepositoryMock.Verify(p => p.DeleteTodo(requestedTodoId), Times.Once());
        }
    }
}
