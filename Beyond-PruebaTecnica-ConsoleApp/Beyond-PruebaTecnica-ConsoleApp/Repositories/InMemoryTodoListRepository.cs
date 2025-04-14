using Beyond_PruebaTecnica_ConsoleApp.Interfaces;

namespace Beyond_PruebaTecnica_ConsoleApp.Repositories
{
    public class InMemoryTodoListRepository : ITodoListRepository
    {
        private int currentId = 1;

        private readonly List<string> categories = new() { "Work", "Personal", "Hobby" };

        public int GetNextId() => currentId++;

        public List<string> GetAllCategories() => categories;
    }
}

