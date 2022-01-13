using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Model;

namespace Computer.Client.Domain.App.ToDoList;

public class ListService : IListService
{
    private ListModel dummy = new()
    {
        Id = "some backend id",
        Items = new ItemModel[]
        {
            new()
            {
                Text = "some backend text"
            },
            new()
            {
                Url = "https://www.google.com"
            },
            new()
            {
                Checked = true,
                ImageUrl = "https://thumbs.gfycat.com/CalmCooperativeKudu-size_restricted.gif"
            }
        }
    };

    private static readonly Lazy<Thread> updateThread = new(() =>
    {
        var thread = new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(2000);
                _callback?.Invoke(new ListChanged
                {
                    ListId = "update thread list id",
                    NewVersion = DateTime.Now.Ticks,
                    OldVersion = DateTime.Now.Ticks
                });
            }
        });
        return thread;
    });

    private static Action<ListChanged> _callback = e => { Console.WriteLine($"{nameof(_callback)} was not set up"); };

    public Task AddItems(IEnumerable<ItemModel> items)
    {
        return Task.CompletedTask;
    }

    public Task DeleteItems(IEnumerable<ItemModel> items)
    {
        return Task.CompletedTask;
    }

    public async Task<TypedResult<ListModel>> GetDefaultListByUserId(string userId)
    {
        if (userId == "throw an error")
        {
            return new TypedResult<ListModel>("User not found");
        }
        var listId = "some list id";
        return new TypedResult<ListModel>(await GetById(userId));
    }

    public Task<ListModel> GetById(string id, ulong? haveVersion = null)
    {
        return Task.FromResult(dummy);
    }

    public IDisposable RegisterForUpdates(string id, Action<ListChanged> callback)
    {
        _callback = callback;
        var t = updateThread.Value; //kick off the update thread
        return new DummySubscription();
    }

    private class DummySubscription : IDisposable
    {
        public void Dispose()
        {
            _callback = e => { Console.WriteLine($"{nameof(_callback)} was disposed"); };
        }
    }
}