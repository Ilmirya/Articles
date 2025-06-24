using Articles.Application.Articles;
using Articles.Domain;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainArticle = Articles.Domain.Article;

namespace Articles.Infrastructure.DataAccess.Repositories;

internal sealed class ArticleRepository(ArticleDbContext context) : IArticleRepository
{
    public async Task<DomainArticle?> Find(Guid id, CancellationToken ct)
    {
        var article =  await context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken: ct);
        
        return article is not null 
            ? ArticleMapper.Map(article)
            : null;
    }

    public Task<List<DomainArticle>> Get(SectionId sectionId, CancellationToken ct) 
        => context.Articles
            .AsNoTracking()
            .Where(x => x.SectionId == sectionId.Value)
            .Select(x => ArticleMapper.Map(x))
            .ToListAsync(cancellationToken: ct);
    
    public async Task Save(DomainArticle article, CancellationToken ct)
    {
        var persistenceArticle = ArticleMapper.Map(article);

        if (await context.Articles.AnyAsync(e => e.Id == persistenceArticle.Id, ct))
        {
            context.Articles.Update(persistenceArticle);
        }
        else
        {
            context.Articles.Add(persistenceArticle);
        }
        await context.SaveChangesAsync(ct);
    }
}