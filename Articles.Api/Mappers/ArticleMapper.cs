using Articles.Api.Models.Responses;
using DomainArticle = Articles.Domain.Article;

namespace Articles.Api.Mappers;

internal static class ArticleMapper
{
    public static ArticleModel Map(DomainArticle source) 
        => new(
            Id: source.Id,
            Name: source.Name,
            CreatedAt: source.CreatedAt,
            UpdatedAt: source.UpdatedAt,
            Tags: source.Tags.Select(t => t.Value).OrderBy(t => t)
        );
}