using Computer.Client.Domain.Contracts.Models;

namespace Computer.Client.Host.App;

public interface IListService
{
    IDisposable RegisterForUpdates(string id, Action<ListChanged> callback);
    Task<ListModel> GetById(string id, ulong? haveVersion);
    Task AddItems(IEnumerable<ItemModel> items);
    Task DeleteItems(IEnumerable<ItemModel> items);
}
