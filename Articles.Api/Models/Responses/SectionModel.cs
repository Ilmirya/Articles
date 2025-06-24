namespace Articles.Api.Models.Responses;

public record SectionModel(Guid Id, string Name, IReadOnlyCollection<string> Tags, IReadOnlyCollection<ArticleModel> Articles);