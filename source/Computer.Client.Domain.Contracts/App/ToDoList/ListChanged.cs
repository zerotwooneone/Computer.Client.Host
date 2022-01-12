namespace Computer.Client.Domain.Contracts.App.ToDoList;

public class ListChanged
{
    public string ListId { get; init; } = "not set";
    public long OldVersion { get; init; }
    public long NewVersion { get; init; }
}