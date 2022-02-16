using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;
[ProtoContract]
#nullable enable
public class ListModel
{
    [ProtoMember(1)]
    public string Id { get; set; } = "not set";
    [ProtoMember(2)]
    public List<ItemModel> Items { get; set; } = new List<ItemModel>();
}