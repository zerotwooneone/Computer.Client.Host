using Computer.Client.Domain.Contracts.Models;
using Computer.Client.Host.App;
using System.Text.Json.Serialization;
using static Computer.Client.Host.App.DummyAppService;

namespace Computer.Client.Host.Controllers;

[JsonSerializable(typeof(ListModel))]
[JsonSerializable(typeof(ListGetByIdParam))]
[JsonSerializable(typeof(AppConnectionRequest))]
[JsonSerializable(typeof(AppDisconnectRequest))]
public partial class HostJsonContext : JsonSerializerContext
{
}