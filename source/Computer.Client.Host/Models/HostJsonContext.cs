using Computer.Client.Domain.Contracts.Models;
using System.Text.Json.Serialization;

namespace Computer.Client.Host.Controllers
{
    [JsonSerializable(typeof(DocumentModel))]
    [JsonSerializable(typeof(ListModel))]
    internal partial class HostJsonContext: JsonSerializerContext
    {
        
    }
}