using CareerGauge.Application.Recommendations;
using Microsoft.AspNetCore.Mvc;

namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(
        IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpGet("{learnerId}")]
    public async Task<IActionResult> GetRecommendations(
        int learnerId)
    {
        var recommendations =
            await _recommendationService
                .GetRecommendationsAsync(learnerId);

        return Ok(recommendations);
    }
}