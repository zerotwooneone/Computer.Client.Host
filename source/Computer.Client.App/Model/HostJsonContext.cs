using System.Text.Json.Serialization;
using Computer.Client.App.Controllers;
using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Contracts.Model;
using Computer.Client.Domain.Contracts.Model.ToDoList;
using static Computer.Client.App.Model.DummyAppService;

namespace Computer.Client.App.Model;

[JsonSerializable(typeof(ListModel))]
[JsonSerializable(typeof(ListGetByIdParam))]
[JsonSerializable(typeof(AppConnectionRequest))]
[JsonSerializable(typeof(StartupModel))]
[JsonSerializable(typeof(StartupParam))]
public partial class HostJsonContext : JsonSerializerContext
{
}