using Articles.Domain;

namespace Articles.Infrastructure.DataAccess.Mappers;
using DomainArticle = Articles.Domain.Article;
using PersistenceArticle = Article;

internal static class ArticleMapper
{
    public static DomainArticle Map(PersistenceArticle source)
        => DomainArticle.LoadFromDb(
            source.Id,
            source.Name,
            source.CreatedAt,
            source.Tags.Select(Tag.Create).ToHashSet(),
            SectionId.Create(source.SectionId),
            source.UpdatedAt);

    public static PersistenceArticle Map(DomainArticle source)
        => new()
        {
            Id = source.Id,
            Name = source.Name,
            CreatedAt = source.CreatedAt,
            Tags = source.Tags.Select(x => x.Value).ToArray(),
            SectionId = source.SectionId.Value,
            UpdatedAt = source.UpdatedAt
        };
}