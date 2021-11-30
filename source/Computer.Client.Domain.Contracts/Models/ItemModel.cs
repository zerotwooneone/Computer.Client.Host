namespace Computer.Client.Domain.Contracts.Models;

public class ItemModel
{
    public string? Text { get; init; } = null;
    public string? Url { get; init; } = null;
    public string? ImageUrl { get; init; } = null;
    public bool Checked { get; init; } = false;
}