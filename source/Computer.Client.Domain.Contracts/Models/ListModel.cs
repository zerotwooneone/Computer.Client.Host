namespace Computer.Client.Domain.Contracts.Models;

public class ListModel
{
    public string Id { get; init; } = "not set";
    public IEnumerable<ItemModel> Items { get; init; } = new ItemModel[] { };
}
