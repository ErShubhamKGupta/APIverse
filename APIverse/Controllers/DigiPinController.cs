using APIverse.Models;
using APIverse.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIverse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DigiPinController : ControllerBase
    {
        private readonly DigiPinService _service;
        private readonly ILogger<DigiPinController> _logger;

        public DigiPinController(ILogger<DigiPinController> logger)
        {
            _logger = logger;
            _service = new DigiPinService(); // In production, use dependency injection
        }

        [HttpPost]
        public IActionResult Post([FromBody] DigiPinRequest request)
        {
            _logger.LogInformation("Received request: Latitude={Latitude}, Longitude={Longitude}", request.Latitude, request.Longitude);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var result = _service.Generate(request.Latitude, request.Longitude);

            if (result.Contains("Out of Range"))
            {
                _logger.LogWarning("Coordinates out of range: {Result}", result);
                return BadRequest(new { message = result });
            }

            _logger.LogInformation("DIGIPIN generated successfully: {DigiPin}", result);
            return Ok(new DigiPinResponse { DigiPin = result });
        }

        [HttpPost("decode")]
        public IActionResult Decode([FromBody] ReverseDigiPinRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DigiPin))
                return BadRequest("DIGIPIN is required.");

            try
            {
                var (lat, lon) = _service.Decode(request.DigiPin);
                return Ok(new ReverseDigiPinResponse { Latitude = lat, Longitude = lon });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to decode DIGIPIN.");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
