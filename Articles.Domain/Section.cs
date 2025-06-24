using Ardalis.GuardClauses;

namespace Articles.Domain;

public class Section
{
    private const int Limit = 256;
    
    public SectionId Id { get; }
    
    public string Name { get; } 
    
    public HashSet<Tag> Tags { get; private set; }
    
    private Section(SectionId id, string name, HashSet<Tag> tags)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.StringTooLong(name, Limit, nameof(name));
        Guard.Against.OutOfRange(tags.Count, nameof(tags), 1, Limit, $"Количество тегов должно быть меньше или равно {Limit}");
        
        Id = id;
        Name = name;
        Tags = tags;
    }
    
    public static Section New(HashSet<Tag> tags)
    {
        var sectionId = SectionId.Create(tags);
        var name = CreateName(tags);
        return new Section(sectionId, name, tags);
    }
    
    public static Section LoadFromDb(SectionId id, string name, HashSet<Tag> tags)
        => new(id, name, tags);

    private static string CreateName(HashSet<Tag> tags)
    {
        var name = TagsConcatenator.Concatenate(tags);
        return name.Length < Limit ? name : name[..Limit];
    }
}