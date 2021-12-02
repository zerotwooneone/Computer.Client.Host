using Computer.Client.Domain.Contracts.Models;
using Computer.Client.Host.App;

namespace Computer.Client.Domain.ToDoList;

public class ListService : IListService
{
    private ListModel dummy = new ListModel
    {
        Id = "some backend id",
        Items = new ItemModel[]
        {
                new ItemModel{
                    Text = "some backend text",
                },
                new ItemModel{
                    Url = "https://www.google.com",
                },
                new ItemModel{
                    Checked = true,
                    ImageUrl = "https://thumbs.gfycat.com/CalmCooperativeKudu-size_restricted.gif",
                },
        }
    };

    private static readonly Lazy<Thread> updateThread = new Lazy<Thread>(() =>
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
                    OldVersion = DateTime.Now.Ticks,
                });
            }
        });
        return thread;
    });
    private static Action<ListChanged> _callback = e =>
    {
        Console.WriteLine($"{nameof(_callback)} was not set up");
    };

    public Task AddItems(IEnumerable<ItemModel> items)
    {
        return Task.CompletedTask;
    }

    public Task DeleteItems(IEnumerable<ItemModel> items)
    {
        return Task.CompletedTask;
    }

    public async Task<ListModel> GetById(string id, ulong? haveVersion)
    {
        return dummy;
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
            _callback = e =>
            {
                Console.WriteLine($"{nameof(_callback)} was disposed");
            };
        }
    }
}
