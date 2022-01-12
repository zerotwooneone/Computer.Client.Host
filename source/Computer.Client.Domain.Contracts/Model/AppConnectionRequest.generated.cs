#nullable enable
namespace Computer.Client.Domain.Contracts.Model;
public partial class AppConnectionRequest
{
    public string InstanceId { get; init; }

    public string AppId { get; init; }
}