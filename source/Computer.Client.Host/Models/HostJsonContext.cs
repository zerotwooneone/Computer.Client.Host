using Computer.Client.Domain.Contracts.Models;
using System.Text.Json.Serialization;
using static Computer.Client.Host.App.DummyAppService;

namespace Computer.Client.Host.Controllers
{
    [JsonSerializable(typeof(ListModel))]
    [JsonSerializable(typeof(ListGetByIdParam))]
    public partial class HostJsonContext: JsonSerializerContext
    {
        
    }
}