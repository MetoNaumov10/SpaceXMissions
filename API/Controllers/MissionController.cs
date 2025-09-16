using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly HttpClient _client;

        public MissionController(IHttpClientFactory httpFactory)
        {
            _client = httpFactory.CreateClient("spacex");
        }

        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatest()
        {
            var resp = await _client.GetAsync("launches/latest");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("upcoming")]
        [Authorize]
        public async Task<IActionResult> GetUpcoming()
        {
            var resp = await _client.GetAsync("launches/upcoming");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("past")]
        [Authorize]
        public async Task<IActionResult> GetPast()
        {
            var resp = await _client.GetAsync("launches/past");
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // Generic proxy allowing client to request type via query param
        [HttpGet("bytype")]
        [Authorize]
        public async Task<IActionResult> GetByType([FromQuery] string type)
        {
            string? endpoint = type.ToLowerInvariant() switch
            {
                "latest" => "launches/latest",
                "upcoming" => "launches/upcoming",
                "past" => "launches/past",
                _ => null
            };
            if (endpoint == null) return BadRequest("Unknown type. Use latest|upcoming|past");
            var resp = await _client.GetAsync(endpoint);
            if (!resp.IsSuccessStatusCode) return StatusCode((int)resp.StatusCode, "SpaceX API error");
            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
