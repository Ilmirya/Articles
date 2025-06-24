using Articles.Api.Models.Responses;

namespace Articles.Api.Mappers;
using DomainSection = Domain.Section;
using DomainArticle = Domain.Article;

internal static class SectionMapper
{
    public static SectionModel Map(DomainSection section, List<DomainArticle> articles) 
        => new()
        {
            Id = section.Id.Value,
            Name = section.Name,
            Articles = articles.Select(ArticleMapper.Map).ToList()
        };
}