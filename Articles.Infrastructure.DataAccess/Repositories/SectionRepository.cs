using Articles.Application.Sections;
using Articles.Domain;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainSection = Articles.Domain.Section;

namespace Articles.Infrastructure.DataAccess.Repositories;

internal sealed class SectionRepository(ArticleDbContext context) : ISectionRepository
{
    public async Task<DomainSection?> Find(SectionId sectionId, CancellationToken ct = default)
    {
        var section = await context.Sections
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sectionId.Value, ct);
        
        return section is not null 
            ? SectionMapper.Map(section)
            : null;
    }

    public Task<List<DomainSection>> Get(SectionFilter filter, CancellationToken ct = default)
    {
        return context.Sections
            .AsNoTracking()
            .Include(s => s.Articles)
            .OrderByDescending(s => s.Articles.Count)
            .Skip(filter.StartRownum)
            .Take(filter.RowCount)
            .Select(x => SectionMapper.Map(x))
            .ToListAsync(ct);
    }

    public async Task Save(DomainSection section, CancellationToken ct = default)
    {
        var persistenceSection = SectionMapper.Map(section);

        if (await context.Sections.AnyAsync(e => e.Id == persistenceSection.Id, ct))
        {
            context.Sections.Update(persistenceSection);
        }
        else
        {
            context.Sections.Add(persistenceSection);
        }
        await context.SaveChangesAsync(ct);
    }
}