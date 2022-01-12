using System.Text.Json;
using Computer.Client.Domain.Contracts.App;
using Computer.Client.Domain.Contracts.App.ToDoList;

namespace Computer.Client.App.Model;

/// <summary>
/// This is a placeholder. This class pretends to be a router that converts json requests into app calls
/// </summary>
public class DummyAppService : IAppService
{
    private readonly IListService listService;
    private readonly HostJsonContext jsonContext;

    public DummyAppService(
        IListService listService,
        HostJsonContext jsonContext)
    {
        this.listService = listService;
        this.jsonContext = jsonContext;
    }

    public async Task<string> JsonFunction(string appName, string methodName, string? param = null)
    {
        switch (appName)
        {
            case "ToDoList":
                return await ListService(methodName, param);
            default:
                throw new ArgumentException($"Unknown App Name {appName}");
        }
    }

    private async Task<string> ListService(string methodName, string? json)
    {
        switch (methodName)
        {
            case "GetById":
                if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException($"Missing param {nameof(json)}");
                var param = JsonSerializer.Deserialize<ListGetByIdParam?>(json, jsonContext.ListGetByIdParam!);
                if (param == null) throw new ArgumentException($"Invalid param {nameof(param)}");
                if (string.IsNullOrWhiteSpace(param.Id))
                    throw new ArgumentException($"Missing param {nameof(param.Id)}");
                var list = await listService.GetById(param.Id, param.HaveVersion);
                var result = JsonSerializer.Serialize(list, jsonContext.ListModel);
                return result;
            default:
                throw new ArgumentException($"Unknown Method Name {methodName}");
        }
    }

    public class ListGetByIdParam
    {
        public string? Id { get; set; }
        public ulong? HaveVersion { get; set; }
    }
}