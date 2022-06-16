using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;

[ProtoContract]
public class ListResponse
{
    [ProtoMember(1)]
    public string? Message { get; set; }
}