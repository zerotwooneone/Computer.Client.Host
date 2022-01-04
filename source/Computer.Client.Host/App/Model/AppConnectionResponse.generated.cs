#nullable enable
using ProtoBuf;

namespace Computer.Client.Host.App.Model;
[ProtoContract]
public partial class AppConnectionResponse
{
    [ProtoMember(1)]
    public string? InstanceId { get; init; }
}