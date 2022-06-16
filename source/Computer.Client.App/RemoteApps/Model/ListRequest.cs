using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;

[ProtoContract]
public class ListRequest
{
    [ProtoMember(1)]
    public string? Message { get; set; }
}