namespace Computer.Client.Domain.Contracts.Models;

public class ItemModel
{
    public string? Text { get; set; } = null;
    public string? Url { get; set; } = null;
    public string? ImageUrl { get; set; } = null;
    public bool Checked { get; set; } = false;
}