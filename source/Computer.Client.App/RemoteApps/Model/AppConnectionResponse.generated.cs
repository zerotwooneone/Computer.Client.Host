#nullable enable
using ProtoBuf;

namespace Computer.Client.App.RemoteApps.Model;
[ProtoContract]
public partial class AppConnectionResponse
{
    [ProtoMember(1)]
    public string? InstanceId { get; init; }
}