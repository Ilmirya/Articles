namespace Articles.Infrastructure.DataAccess;

internal sealed class Section
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required ICollection<string> Tags { get; set; }
    public ICollection<Article> Articles { get; set; }
}