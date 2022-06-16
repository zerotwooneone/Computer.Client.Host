using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;

[ProtoContract]
public class ListEvent
{
    [ProtoMember(1)]
    public string? Message { get; set; }
}