using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;
[ProtoContract]
#nullable enable
public class DefaultListResponse
{
    [ProtoMember(1)]
    public bool Success { get; init; }

    [ProtoMember(2)]
    public ListModel? List { get; init; }
}