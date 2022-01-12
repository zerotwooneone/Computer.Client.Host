namespace Computer.Client.Domain.Contracts.App.ToDoList;

public interface IListService
{
    IDisposable RegisterForUpdates(string id, Action<ListChanged> callback);
    Task<ListModel> GetById(string id, ulong? haveVersion);
    Task AddItems(IEnumerable<ItemModel> items);
    Task DeleteItems(IEnumerable<ItemModel> items);
}