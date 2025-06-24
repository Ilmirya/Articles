using Articles.Api.Mappers;
using Articles.Api.Models.Requests;
using Articles.Api.Models.Responses;
using Articles.Application.Articles;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController(
    ArticleService articleService,
    IArticleRepository articleRepository)
    : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArticleModel>> Get(Guid id, CancellationToken ct)
    {
        var article = await articleRepository.Find(id, ct);
        return article is null 
            ? NotFound() 
            : Ok(ArticleMapper.Map(article));
    }

    [HttpPost]
    public async Task<ActionResult<ArticleModel>> Create([FromBody] UpdateArticleRequest request, CancellationToken ct)
    {
        var articleId = await articleService.Create(request.Title, request.Tags, ct);
            
        var article = await articleRepository.Find(articleId, ct);
        return article is null 
            ? StatusCode(StatusCodes.Status500InternalServerError, "Internal server error") 
            : CreatedAtAction(nameof(Get), new { id = articleId }, ArticleMapper.Map(article));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateArticleRequest request, CancellationToken ct)
    {
        await articleService.Update(id, request.Title, request.Tags, ct);
        return NoContent();
    }
}