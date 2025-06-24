namespace Articles.Domain;

public class Section
{
    public SectionId Id { get; }
    
    public string Name { get; } 
    
    public HashSet<Tag> Tags { get; private set; }
    
    private Section(SectionId id, string name, HashSet<Tag> tags)
    {
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
        return name.Length < 256 ? name : name[..256];
    }
}