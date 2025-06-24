using Ardalis.GuardClauses;

namespace Articles.Domain;

public class Article
{
    public Guid Id { get; }
    
    public string Name { get; private set; } 
    
    public DateTime CreatedAt { get; }
    
    public DateTime? UpdatedAt { get; private set; }
    
    public HashSet<Tag> Tags { get; private set; }
    
    public SectionId SectionId { get; private set; }
    
    private Article(
        Guid id, 
        string name, 
        DateTime createdAt, 
        HashSet<Tag> tags, 
        SectionId sectionId, 
        DateTime? updatedAt = null)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.StringTooLong(name, 256, nameof(name));
        Id = id;
        Name = name;
        Tags = tags;
        SectionId = sectionId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Article New(string name, SectionId sectionId, HashSet<Tag> tags) 
        => new(Guid.NewGuid(), name, createdAt: DateTime.UtcNow, tags, sectionId);

    public void Update(string name, HashSet<Tag> tags, SectionId sectionId)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.StringTooLong(name, 256, nameof(name));
        Name = name;
        Tags = tags;
        SectionId = sectionId;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Article LoadFromDb(
        Guid id, 
        string title, 
        DateTime createdAt, 
        HashSet<Tag> tags, 
        SectionId sectionId,
        DateTime? updatedAt)
        => new(id, title, createdAt, tags, sectionId, updatedAt);
}