using Beyond_PruebaTecnica_ConsoleApp.Exceptions;
using Beyond_PruebaTecnica_ConsoleApp.Interfaces;
using Beyond_PruebaTecnica_ConsoleApp.Services;
using Xunit;

namespace Beyond_UnitTests
{
    public class TodoListTests
    {
        private class ToDoListTestsRepository : ITodoListRepository
        {
            private int currentId = 0;
            private readonly List<string> categories = new() { "Work", "Personal", "Hobby" };

            public int GetNextId() => ++currentId;
            public List<string> GetAllCategories() => categories;
        }

        [Fact]
        public void HappyPath_ShouldAddAndPrintCorrectly()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();
            todoList.AddItem(id, "Complete Project Report", "Finish the final report for the project", "Work");
            todoList.RegisterProgression(id, new DateTime(2025, 3, 18), 30);
            todoList.RegisterProgression(id, new DateTime(2025, 3, 19), 50);
            todoList.RegisterProgression(id, new DateTime(2025, 3, 20), 20);

            Assert.Throws<UpdateNotAllowedException>(() => todoList.UpdateItem(id, "Cambio"));
                        
            var item = todoList.Find(id);
            Assert.True(item.IsCompleted);
        }

        [Fact]
        public void ShouldFail_WhenDateIsNotInOrder()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();
            todoList.AddItem(id, "Test", "Test Desc", "Work");
            todoList.RegisterProgression(id, new DateTime(2025, 3, 20), 50);

            Assert.Throws<InvalidProgressDateException>(() =>
                todoList.RegisterProgression(id, new DateTime(2025, 3, 19), 20)
            );
        }

        [Fact]
        public void ShouldFail_WhenPercentIsInvalid()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();
            todoList.AddItem(id, "Test", "Test Desc", "Work");

            Assert.Throws<InvalidProgressException>(() =>
                todoList.RegisterProgression(id, DateTime.Now, 0));

            Assert.Throws<InvalidProgressException>(() =>
                todoList.RegisterProgression(id, DateTime.Now, 100));
        }

        [Fact]
        public void ShouldFail_WhenTotalExceeds100()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();
            todoList.AddItem(id, "Test", "Test Desc", "Work");
            todoList.RegisterProgression(id, DateTime.Now.AddDays(-1), 60);

            Assert.Throws<ProgressOverflowException>(() =>
                todoList.RegisterProgression(id, DateTime.Now, 50));
        }

        [Fact]
        public void ShouldFail_WhenUpdatingOrRemovingOver50Percent()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();
            todoList.AddItem(id, "Test", "Test Desc", "Work");
            todoList.RegisterProgression(id, DateTime.Now, 60);

            Assert.Throws<UpdateNotAllowedException>(() => todoList.UpdateItem(id, "New Description"));
            Assert.Throws<RemoveNotAllowedException>(() => todoList.RemoveItem(id));
        }

        [Fact]
        public void ShouldFail_WhenCategoryIsInvalid()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var invalidCategory = "Fitness";

            Assert.Throws<InvalidCategoryException>(() =>
                todoList.AddItem(repo.GetNextId(), "Test", "Test Desc", invalidCategory));
        }

        [Fact]
        public void ShouldAllow100PercentExactly()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();

            todoList.AddItem(id, "Final Task", "Desc", "Work");
            todoList.RegisterProgression(id, DateTime.Now.AddDays(-2), 30);
            todoList.RegisterProgression(id, DateTime.Now.AddDays(-1), 70);

            var item = todoList.Find(id);
            Assert.True(item.IsCompleted);
            Assert.Equal(100, item.TotalProgress);
        }

        [Fact]
        public void ShouldFail_WhenProgressDateIsEqual()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();

            var date = DateTime.Today;
            todoList.AddItem(id, "Test", "Desc", "Work");
            todoList.RegisterProgression(id, date, 20);

            var ex = Assert.Throws<InvalidProgressDateException>(() =>
                todoList.RegisterProgression(id, date, 10));

            Assert.Contains("fecha debe ser mayor", ex.Message);
        }

        [Fact]
        public void ShouldUpdateDescription_WhenProgressIsUnder50()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = repo.GetNextId();

            todoList.AddItem(id, "Test", "Original Desc", "Personal");
            todoList.RegisterProgression(id, DateTime.Now, 30);

            todoList.UpdateItem(id, "Updated Desc");
            var item = todoList.Find(id);
            Assert.Equal("Updated Desc", item.Description);
        }

        [Fact]
        public void ShouldThrow_WhenRemovingNonExistingItem()
        {
            ITodoListRepository repo = new ToDoListTestsRepository();
            TodoList todoList = new TodoList(repo);

            var id = 999;

            var ex = Assert.Throws<ItemNotFoundException>(() => todoList.RemoveItem(id));
            Assert.Contains($"Id={id}", ex.Message);
        }

        [Fact]
        public void GetNextId_ShouldIncrementCorrectly()
        {
            var repo = new ToDoListTestsRepository();

            Assert.Equal(1, repo.GetNextId());
            Assert.Equal(2, repo.GetNextId());
            Assert.Equal(3, repo.GetNextId());
        }

        [Fact]
        public void GetAllCategories_ShouldContainDefaults()
        {
            var repo = new ToDoListTestsRepository();

            var categories = repo.GetAllCategories();

            Assert.Contains("Work", categories);
            Assert.Contains("Personal", categories);
            Assert.Contains("Hobby", categories);
            Assert.Equal(3, categories.Count);
        }
    }
}
