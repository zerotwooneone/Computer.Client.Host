#nullable enable
namespace Computer.Client.Domain.Model;
public partial class AppConnectionRequest
{
    public string InstanceId { get; init; }

    public string AppId { get; init; }
}