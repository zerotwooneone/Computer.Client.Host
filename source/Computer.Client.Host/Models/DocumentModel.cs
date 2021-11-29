using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Computer.Client.Host.Controllers
{
    public class DocumentModel
    {
        public Dictionary<string, string> Strings { get; set; } = new();
        public Dictionary<string, int> Ints { get; set; } = new();
        public Dictionary<string, decimal> Decimals { get; set; } = new();
    }
    
    [JsonSerializable(typeof(DocumentModel))]
    internal partial class DocumentModelContext: JsonSerializerContext
    {
        
    }
}