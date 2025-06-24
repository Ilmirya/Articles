using Articles.Domain;

namespace Articles.Infrastructure.DataAccess.Mappers;
using DomainSection = Articles.Domain.Section;
using PersistenceSection = Section;

internal static class SectionMapper
{
    public static DomainSection Map(PersistenceSection source)
        => DomainSection.LoadFromDb(SectionId.Create(source.Id), source.Name, source.Tags.Select(Tag.Create).ToHashSet());

    public static PersistenceSection Map(DomainSection source)
        => new()
        {
            Id = source.Id.Value,
            Name = source.Name,
            Tags = source.Tags.Select(x => x.Value).ToArray()
        };
}