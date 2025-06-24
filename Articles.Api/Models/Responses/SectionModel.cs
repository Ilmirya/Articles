namespace Articles.Api.Models.Responses;

public class SectionModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public IReadOnlyCollection<ArticleModel> Articles { get; set; }
}