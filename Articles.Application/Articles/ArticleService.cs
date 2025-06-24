using Articles.Application.Sections;
using Articles.Domain;

namespace Articles.Application.Articles;

public sealed class ArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ISectionRepository _sectionRepository;

    public ArticleService(
        IArticleRepository articleRepository,
        ISectionRepository sectionRepository)
    {
        _articleRepository = articleRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<Guid> Create(string name, IEnumerable<string> tagNames, CancellationToken ct)
    {
        var tags = tagNames.Select(Tag.Create).ToHashSet();
        
        var section = await GetOrCreateSection(tags, ct);
        var article = Article.New(name, section.Id, tags);
        
        await _articleRepository.Save(article, ct);
        
        return article.Id;
    }

    public async Task Update(Guid id, string name, IEnumerable<string> tagNames, CancellationToken ct)
    {
        var article = await _articleRepository.Find(id, ct);
        if (article == null)
            throw new ObjectNotFoundException(nameof(Article));
        var tags = tagNames.Select(Tag.Create).ToHashSet();
        
        var section = await GetOrCreateSection(tags, ct);
        article.Update(name, tags, section.Id);
        
        await _articleRepository.Save(article, ct);
    }
    
    private async Task<Section> GetOrCreateSection(HashSet<Tag> tags, CancellationToken ct)
    {
        var section = await _sectionRepository.Find(SectionId.Create(tags), ct);
        if (section is not null)
        {
            return section;
        }
        
        section = Section.New(tags);
        await _sectionRepository.Save(section, ct);
        
        return section;
    }
}