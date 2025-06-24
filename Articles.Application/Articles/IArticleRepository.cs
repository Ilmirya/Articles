using Articles.Domain;

namespace Articles.Application.Articles;

public interface IArticleRepository
{
    Task<Article?> Find(Guid id, CancellationToken ct = default);
    
    Task<List<Article>> Get(SectionId sectionId, CancellationToken ct = default);
    
    Task Save(Article article, CancellationToken ct = default);
}