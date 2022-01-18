namespace Computer.Client.Domain.Contracts.Model.ToDoList;
#nullable enable
public class DefaultListResponse
{
    public bool Success { get; init; }

    public ListModel? List { get; init; }
}