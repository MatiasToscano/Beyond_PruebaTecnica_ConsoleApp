using Beyond_PruebaTecnica_ConsoleApp.Interfaces;
using Beyond_PruebaTecnica_ConsoleApp.Repositories;
using Beyond_PruebaTecnica_ConsoleApp.Services;

ITodoListRepository repo = new InMemoryTodoListRepository();
TodoList todoList = new(repo);

int id1 = repo.GetNextId();
todoList.AddItem(id1, "Complete Project Report", "Finish the final report for the project", "Work");
todoList.RegisterProgression(id1, new DateTime(2025, 3, 18), 30);
todoList.RegisterProgression(id1, new DateTime(2025, 3, 19), 50);
todoList.RegisterProgression(id1, new DateTime(2025, 3, 20), 20);

int id2 = repo.GetNextId();
todoList.AddItem(id2, "Buy Groceries", "Pick up fruits and veggies", "Personal");
todoList.RegisterProgression(id2, new DateTime(2025, 4, 1), 40);
todoList.RegisterProgression(id2, new DateTime(2025, 4, 2), 30);

int id3 = repo.GetNextId();
todoList.AddItem(id3, "Paint Miniatures", "Finish painting the space marines squad", "Hobby");
todoList.RegisterProgression(id3, new DateTime(2025, 4, 5), 25);
todoList.RegisterProgression(id3, new DateTime(2025, 4, 6), 25);
todoList.RegisterProgression(id3, new DateTime(2025, 4, 7), 50);

todoList.PrintItems();
