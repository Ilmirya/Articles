using Articles.Api.Mappers;
using Articles.Api.Models;
using Articles.Api.Models.Requests;
using Articles.Api.Models.Responses;
using Articles.Application;
using Articles.Application.Articles;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly ILogger<ArticleController> _logger;
    private readonly IArticleRepository _articleRepository;

    public ArticleController(
        ArticleService articleService, 
        ILogger<ArticleController> logger,
        IArticleRepository articleRepository)
    {
        _articleService = articleService;
        _logger = logger;
        _articleRepository = articleRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArticleModel>> Get(Guid id)
    {
        try
        {
            var article = await _articleRepository.Find(id);
            if (article is null)
                return NotFound();
            
            return Ok(ArticleMapper.Map(article));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ArticleModel>> Create([FromBody] CreateArticleRequest createArticleRequest, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var articleId = await _articleService.Create(
                createArticleRequest.Title, 
                createArticleRequest.Tags,
                ct);
            
            var article = await _articleRepository.Find(articleId, ct);
            return CreatedAtAction(nameof(Get), new { id = articleId }, ArticleMapper.Map(article));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating article");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateArticleRequest updateDto, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            await _articleService.Update(
                id, 
                updateDto.Title, 
                updateDto.Tags,
                ct);
            
            return NoContent();
        }
        catch (ObjectNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating article");
            return StatusCode(500, "Internal server error");
        }
    }
}