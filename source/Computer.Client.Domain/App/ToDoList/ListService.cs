using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Bus;
using Computer.Client.Domain.Contracts.Model;
using Computer.Client.Domain.Contracts.Model.ToDoList;
using Computer.Domain.Bus.Contracts;

namespace Computer.Client.Domain.App.ToDoList;

public class ListService : IListService
{
    private readonly IRequestService _requestService;

    public ListService(IRequestService requestService)
    {
        _requestService = requestService;
    }
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
        var request = new DefaultListRequest { UserId = userId };

        var correlationId = Guid.NewGuid().ToString();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var cancellationToken = cts.Token;
        var response = await _requestService.Request<DefaultListRequest, DefaultListResponse>(request, Events.DefaultListRequest, Events.DefaultListResponse, correlationId, cancellationToken: cancellationToken);
        if(response == null || 
            !response.Success || 
            response.Obj == null || 
            response.Obj.List == null)
        {
            return new TypedResult<ListModel>("something went wrong");
        }
        return new TypedResult<ListModel>(response.Obj.List);
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