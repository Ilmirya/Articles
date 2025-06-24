using Articles.Api.Mappers;
using Articles.Api.Models.Requests;
using Articles.Api.Models.Responses;
using Articles.Application.Articles;
using Articles.Application.Sections;
using Articles.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Articles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly ILogger<SectionController> _logger;
    private readonly ISectionRepository _sectionRepository;
    private readonly IArticleRepository _articleRepository;
    
    public SectionController(
        ArticleService articleService, 
        ILogger<SectionController> logger,
        ISectionRepository sectionRepository, 
        IArticleRepository articleRepository)
    {
        _articleService = articleService;
        _logger = logger;
        _sectionRepository = sectionRepository;
        _articleRepository = articleRepository;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SectionModel>> Get(Guid id, CancellationToken ct)
    {
        try
        {
            var section = await _sectionRepository.Find(SectionId.Create(id), ct);
            if (section is null)
                return NotFound();
            
            var articles = await _articleRepository.Get(section.Id, ct);
            return Ok(SectionMapper.Map(section, articles));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sections");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SectionModel>>> Get([FromQuery] GetSectionsRequest request, CancellationToken ct)
    {
        try
        {
            var sectionModels = new List<SectionModel>();
            var sectionFilter = new SectionFilter(request.StartRownum, request.RowCount);
            var sections = await _sectionRepository.Get(sectionFilter, ct);
            foreach (var section in sections)
            {
                var articles = await _articleRepository.Get(section.Id, ct);
                sectionModels.Add(SectionMapper.Map(section, articles));
            }
            
            return Ok(sectionModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sections");
            return StatusCode(500, "Internal server error");
        }
    }
}