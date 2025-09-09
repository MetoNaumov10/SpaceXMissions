using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly IHttpClientFactory _httpFactory;

        public MissionController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        private HttpClient CreateClient() => _httpFactory.CreateClient("spacex");

        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatest()
        {
            var client = CreateClient();
            var resp = await client.GetAsync("launches/latest");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("upcoming")]
        [Authorize]
        public async Task<IActionResult> GetUpcoming()
        {
            var client = CreateClient();
            var resp = await client.GetAsync("launches/upcoming");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("past")]
        [Authorize]
        public async Task<IActionResult> GetPast()
        {
            var client = CreateClient();
            var resp = await client.GetAsync("launches/past");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // Generic proxy allowing client to request type via query param
        [HttpGet("bytype")]
        [Authorize]
        public async Task<IActionResult> GetByType([FromQuery] string type)
        {
            var client = CreateClient();
            string? endpoint = type.ToLowerInvariant() switch
            {
                "latest" => "launches/latest",
                "upcoming" => "launches/upcoming",
                "past" => "launches/past",
                _ => null
            };
            if (endpoint == null) return BadRequest("Unknown type. Use latest|upcoming|past");
            var resp = await client.GetAsync(endpoint);
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
