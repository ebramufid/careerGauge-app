using CareerGauge.Application.Readiness;
using Microsoft.AspNetCore.Mvc;

namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadinessController : ControllerBase
{
    private readonly IReadinessService _readinessService;

    public ReadinessController(
        IReadinessService readinessService)
    {
        _readinessService = readinessService;
    }

    [HttpGet("{learnerId}/career/{careerProfileId}")]
    public async Task<IActionResult> GetReadiness(
        int learnerId,
        int careerProfileId)
    {
        var result = await _readinessService.CalculateAsync(
            learnerId,
            careerProfileId);

        return Ok(result);
    }
}