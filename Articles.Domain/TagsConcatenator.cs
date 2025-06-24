using System.Text;

namespace Articles.Domain;

public static class TagsConcatenator
{
    public static string Concatenate(IEnumerable<Tag> tags)
    {
        var sb = new StringBuilder();
        using var tagsEnumerator = tags.GetEnumerator();

        if (!tagsEnumerator.MoveNext())
        {
            return string.Empty;
        }
        
        sb.Append(tagsEnumerator.Current.Value);
    
        while (tagsEnumerator.MoveNext())
        {
            sb.Append(',').Append(tagsEnumerator.Current.Value);
        }
        return sb.ToString();
    }
}