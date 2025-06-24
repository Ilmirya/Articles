using Articles.Application.Articles;
using Articles.Domain;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainArticle = Articles.Domain.Article;

namespace Articles.Infrastructure.DataAccess.Repositories;

internal sealed class ArticleRepository : IArticleRepository
{
    private readonly ArticleDbContext _context;

    public ArticleRepository(ArticleDbContext context) => _context = context;

    public async Task<DomainArticle?> Find(Guid id, CancellationToken ct)
    {
        var article =  await _context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken: ct);
        
        return article is not null 
            ? ArticleMapper.Map(article)
            : null;
    }

    public Task<List<DomainArticle>> Get(SectionId sectionId, CancellationToken ct) 
        => _context.Articles
            .AsNoTracking()
            .Where(x => x.SectionId == sectionId.Value)
            .Select(x => ArticleMapper.Map(x))
            .ToListAsync(cancellationToken: ct);
    
    public async Task Save(DomainArticle article, CancellationToken ct)
    {
        var persistenceArticle = ArticleMapper.Map(article);

        if (await _context.Articles.AnyAsync(e => e.Id == persistenceArticle.Id, ct))
        {
            _context.Articles.Update(persistenceArticle);
        }
        else
        {
            _context.Articles.Add(persistenceArticle);
        }
        await _context.SaveChangesAsync(ct);
    }
}