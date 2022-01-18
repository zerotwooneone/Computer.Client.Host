namespace Computer.Client.Domain.Contracts.Bus;

public static class Events
{
    public static readonly string GetConnection = "GetConnection";
    public static readonly string CloseConnection = "CloseConnection";
    public static readonly string GetConnectionResponse = "GetConnectionResponse";
    public static readonly string CloseConnectionResponse = "CloseConnectionResponse";
    public static readonly string GetListRequest = "GetListRequest";
    public static readonly string GetListResponse = "GetListResponse";
    public static readonly string DefaultListRequest = "DefaultListRequest";
    public static readonly string DefaultListResponse = "DefaultListResponse";
}