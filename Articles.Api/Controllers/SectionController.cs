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
public class SectionController(
    ISectionRepository sectionRepository,
    IArticleRepository articleRepository)
    : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SectionModel>> Get(Guid id, CancellationToken ct)
    {
        var section = await sectionRepository.Find(SectionId.Create(id), ct);
        if (section is null)
            return NotFound();
            
        var articles = await articleRepository.Get(section.Id, ct);
        return Ok(SectionMapper.Map(section, articles));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SectionModel>>> Get([FromQuery] GetSectionsRequest request, CancellationToken ct)
    {
        var sectionModels = new List<SectionModel>();
        var sectionFilter = new SectionFilter(request.StartRownum, request.RowCount);
        var sections = await sectionRepository.Get(sectionFilter, ct);
        foreach (var section in sections)
        {
            var articles = await articleRepository.Get(section.Id, ct);
            sectionModels.Add(SectionMapper.Map(section, articles));
        }
            
        return Ok(sectionModels);
    }
}