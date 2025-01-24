using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Helpers;
using ApiKwalifikacyjne.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiKwalifikacyjne.Controllers;

[ApiController]
[Route("[controller]")]
public class TagsController : ControllerBase
{
    private readonly ILogger<TagsController> _logger;
    private readonly IDataService _dataService;

    public TagsController(ILogger<TagsController> logger, IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    [HttpGet(Name = "GetTags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Tag>>> Get(string? field = "name", string? queryOrder = "asc",
        int? page = 1)
    {
        _logger.LogInformation($"Tags queried with params field: {field}, order: {queryOrder}, page: {page}");
        try
        {
            return Ok(await _dataService.GetTags(field, queryOrder, page));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}