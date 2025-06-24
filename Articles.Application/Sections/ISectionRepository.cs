using Articles.Domain;

namespace Articles.Application.Sections;

public interface ISectionRepository
{
    Task<Section?> Find(SectionId sectionId, CancellationToken ct = default);
    
    Task<List<Section>> Get(SectionFilter filter, CancellationToken ct = default);
    Task Save(Section section, CancellationToken ct = default);
}