namespace Articles.Api.Models.Responses;

public class ArticleModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}