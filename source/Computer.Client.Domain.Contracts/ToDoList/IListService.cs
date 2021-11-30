using Computer.Client.Domain.Contracts.Models;

namespace Computer.Client.Domain.Contracts.ToDoList;

public interface IListService
{
    IDisposable RegisterForUpdates(string id, Action<ListChanged> callback);
    Task<ListModel> GetById(string id);
    Task AddItems(IEnumerable<ItemModel> items);
    Task DeleteItems(IEnumerable<ItemModel> items);
}
