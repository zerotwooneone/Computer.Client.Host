using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;
[ProtoContract]
#nullable enable
public class DefaultListRequest
{
    [ProtoMember(1)]
    public string UserId { get; init; } = "not set";
}