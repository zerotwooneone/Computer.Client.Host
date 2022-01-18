using Computer.Client.Domain.Contracts.Model;
using Computer.Client.Domain.Contracts.Model.ToDoList;

namespace Computer.Client.Domain.Contracts.App.ToDoList;

public interface IListService
{
    IDisposable RegisterForUpdates(string id, Action<ListChanged> callback);
    //Task<ListModel> GetById(string id, ulong? haveVersion = null);
    Task AddItems(IEnumerable<ItemModel> items);
    Task DeleteItems(IEnumerable<ItemModel> items);
    Task<TypedResult<ListModel>> GetDefaultListByUserId(string userId);
}