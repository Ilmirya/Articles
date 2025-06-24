using Articles.Application.Exceptions;
using Articles.Domain;

namespace Articles.Application.Articles;

public sealed class ArticleService(IArticleUnitOfWork unitOfWork)
{
    public async Task<Guid> Create(string name, IEnumerable<string> tagNames, CancellationToken ct)
    {
        var tags = tagNames.Select(Tag.Create).ToHashSet();
        
        await unitOfWork.StartTransaction(ct);
        try
        {
            var section = await GetOrCreateSection(tags, ct);
            var article = Article.New(name, section.Id, tags);
            
            await unitOfWork.Articles.Save(article, ct);
            await unitOfWork.Commit(ct);
            return article.Id;
        }
        catch
        {
            await unitOfWork.Rollback(ct);
            throw;
        }
    }

    public async Task Update(Guid id, string name, IEnumerable<string> tagNames, CancellationToken ct)
    {
        var article = await unitOfWork.Articles.Find(id, ct);
        if (article == null)
            throw new ObjectNotFoundException(nameof(Article));
        var tags = tagNames.Select(Tag.Create).ToHashSet();
        
        await unitOfWork.StartTransaction(ct);
        try
        {
            var section = await GetOrCreateSection(tags, ct);
            article.Update(name, tags, section.Id);

            await unitOfWork.Articles.Save(article, ct);
            await unitOfWork.Commit(ct);
        }
        catch
        {
            await unitOfWork.Rollback(ct);
            throw;
        }
    }
    
    private async Task<Section> GetOrCreateSection(HashSet<Tag> tags, CancellationToken ct)
    {
        var section = await unitOfWork.Sections.Find(SectionId.Create(tags), ct);
        if (section is not null)
        {
            return section;
        }
        
        section = Section.New(tags);
        await unitOfWork.Sections.Save(section, ct);
        
        return section;
    }
}