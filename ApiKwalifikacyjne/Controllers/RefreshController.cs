using ApiKwalifikacyjne.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiKwalifikacyjne.Controllers;

[ApiController]
[Route("[controller]")]
public class RefreshController : ControllerBase
{
    private readonly ILogger<TagsController> _logger;
    private readonly IDataService _dataService;

    public RefreshController(ILogger<TagsController> logger, IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    [HttpGet(Name = "Refresh downloaded data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get()
    {
        _logger.LogInformation("Refreshing downloaded data");
        try
        {
            await _dataService.FetchData();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}