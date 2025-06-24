namespace Articles.Api.Models.Responses;

public record ArticleModel(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<string> Tags);