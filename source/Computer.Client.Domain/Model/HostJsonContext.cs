using System.Text.Json.Serialization;
using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Model;
using static Computer.Client.Domain.App.DummyAppService;

namespace Computer.Client.Domain.Model;

[JsonSerializable(typeof(ListModel))]
[JsonSerializable(typeof(ListGetByIdParam))]
[JsonSerializable(typeof(AppConnectionRequest))]
public partial class HostJsonContext : JsonSerializerContext
{
}