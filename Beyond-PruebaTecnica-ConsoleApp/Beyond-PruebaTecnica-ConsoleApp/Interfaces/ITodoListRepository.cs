namespace Beyond_PruebaTecnica_ConsoleApp.Interfaces
{
    public interface ITodoListRepository
    {
        int GetNextId();
        List<string> GetAllCategories();
    }
}
