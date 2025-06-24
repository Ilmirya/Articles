using Ardalis.GuardClauses;

namespace Articles.Domain;

public sealed class Tag : ValueObject, IComparable<Tag>
{
    public string Value { get; }
    
    public static Tag Create(string value)
        => new(value);
    
    private Tag(string value)
    {
        Guard.Against.NullOrEmpty(value, nameof(value));
        Guard.Against.StringTooLong(value, 256, nameof(Tag));
        Value = value.Trim().ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public int CompareTo(Tag? other) 
        => other is null 
            ? 1 
            : string.Compare(Value, other.Value, StringComparison.Ordinal);
}