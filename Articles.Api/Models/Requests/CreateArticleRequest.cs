using System.ComponentModel.DataAnnotations;

namespace Articles.Api.Models.Requests;

public class CreateArticleRequest
{
    [Required]
    [MaxLength(256)]
    public required string Title { get; set; }
    
    [MaxLength(256)]
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}