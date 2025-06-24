using Ardalis.GuardClauses;

namespace Articles.Domain;

public class Article
{
    private const int Limit = 256;
    
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
        Guard.Against.StringTooLong(name, Limit, nameof(name));
        Guard.Against.OutOfRange(tags.Count, nameof(tags), 0, Limit, $"Количество тегов должно быть меньше или равно {Limit}");
        
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
        Guard.Against.StringTooLong(name, Limit, nameof(name));
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