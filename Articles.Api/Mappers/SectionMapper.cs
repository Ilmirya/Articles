using Articles.Api.Models.Responses;

namespace Articles.Api.Mappers;
using DomainSection = Domain.Section;
using DomainArticle = Domain.Article;

internal static class SectionMapper
{
    public static SectionModel Map(DomainSection section, List<DomainArticle> articles)
        => new(
            section.Id.Value,
            section.Name, 
            section.Tags.Select(x => x.Value).ToList(), 
            articles.Select(ArticleMapper.Map).ToList());
}