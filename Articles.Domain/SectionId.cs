using System.Security.Cryptography;
using System.Text;
using Ardalis.GuardClauses;

namespace Articles.Domain;

public sealed class SectionId : ValueObject
{
    public Guid Value { get; }
    
    public static SectionId Create(IReadOnlyCollection<Tag> tags)
    {
        Guard.Against.NullOrEmpty(tags, nameof(tags));
        
        var sortedUniqueTags = tags
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
        
        var concatenatedTags = TagsConcatenator.Concatenate(sortedUniqueTags);
        
        Guard.Against.NullOrEmpty(concatenatedTags, nameof(concatenatedTags));
        
        var sectionId = CreateSectionId(concatenatedTags);
        return new SectionId(sectionId);
    }
    
    public static SectionId Create(Guid value)
        => new(value);

    private SectionId(Guid value)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private static Guid CreateSectionId(string tags)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(tags));
        return new Guid(hash);
    }

}