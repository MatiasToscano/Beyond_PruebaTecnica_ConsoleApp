using Beyond_PruebaTecnica_ConsoleApp.Exceptions;
using Beyond_PruebaTecnica_ConsoleApp.Interfaces;
using Beyond_PruebaTecnica_ConsoleApp.Models;

namespace Beyond_PruebaTecnica_ConsoleApp.Services
{
    public class TodoList : ITodoList
    {
        private readonly List<TodoItem> items = new();
        private readonly ITodoListRepository repository;
        private const string Separator = "-----------------------------------------------------------------------";

        public TodoList(ITodoListRepository repository)
        {
            this.repository = repository;
        }

        public void AddItem(int id, string title, string description, string category)
        {
            if (!repository.GetAllCategories().Contains(category))
                throw new InvalidCategoryException();

            items.Add(new TodoItem(id, title, description, category));
        }

        public void UpdateItem(int id, string description)
        {
            var item = Find(id);
            item.UpdateDescription(description);
        }

        public void RemoveItem(int id)
        {
            var item = Find(id);
            if (item.TotalProgress > 50)
                throw new RemoveNotAllowedException();

            items.Remove(item);
        }

        public void RegisterProgression(int id, DateTime dateTime, decimal percent)
        {
            var item = Find(id);
            item.AddProgression(dateTime, percent);
        }

        public void PrintItems()
        {
            foreach (var item in items.OrderBy(i => i.Id))
            {
                Console.WriteLine(item.Print());
                Console.WriteLine(Separator);
            }
        }

        public TodoItem Find(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                throw new ItemNotFoundException(id);

            return item;
        }
    }
}