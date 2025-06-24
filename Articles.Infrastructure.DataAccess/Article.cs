namespace Articles.Infrastructure.DataAccess;

internal sealed class Article
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public string[] Tags { get; set; }
    
    public Guid SectionId { get; set; }
    public Section Section { get; set; }
}