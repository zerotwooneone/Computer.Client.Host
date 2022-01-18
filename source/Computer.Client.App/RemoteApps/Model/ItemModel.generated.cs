using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;
[ProtoContract]
#nullable enable
public class ItemModel
{
    [ProtoMember(1)]
    public string? Text { get; set; } = null;
    [ProtoMember(2)]
    public string? Url { get; set; } = null;
    [ProtoMember(3)]
    public string? ImageUrl { get; set; } = null;
    [ProtoMember(4)]
    public bool Checked { get; set; } = false;
}