using Microsoft.AspNetCore.Mvc;
using MixvelTest.Models;
using MixvelTest.Services.Interfaces;

namespace MixvelTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            // Проверяем входные данные на валидность, если необходимо
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var searchResponse = await _searchService.SearchAsync(request);

                return Ok(searchResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("IsAvailable")]
        public async Task<IActionResult> IsAvailable()
        {
            try
            {
                var isAvailable = await _searchService.IsAvailableAsync();

                if (isAvailable)
                {
                    return Ok("All services are available.");
                }
                else
                {
                    return StatusCode(503, "One or more services are unavailable.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}