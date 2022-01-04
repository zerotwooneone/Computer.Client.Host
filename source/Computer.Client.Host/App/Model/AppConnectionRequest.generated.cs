#nullable enable
using ProtoBuf;

namespace Computer.Client.Host.App.Model;
[ProtoContract]
public partial class AppConnectionRequest
{
    [ProtoMember(1)]
    public string InstanceId { get; init; }

    [ProtoMember(2)]
    public string AppId { get; init; }
}