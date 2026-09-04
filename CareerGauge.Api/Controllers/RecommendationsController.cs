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

    [HttpGet("{learnerId}/career/{careerProfileId}")]
    public async Task<IActionResult> GetRecommendationDetails(
        int learnerId,
        int careerProfileId)
    {
        var result =
            await _recommendationService
                .GetRecommendationDetailsAsync(
                    learnerId,
                    careerProfileId);

        return Ok(result);
    }


    [HttpGet("{learnerId}/compare")]
    public async Task<IActionResult> CompareCareers(
    int learnerId,
    [FromQuery] List<int> careerProfileIds)
    {
        var comparisons =
            await _recommendationService
                .GetComparisonAsync(
                    learnerId,
                    careerProfileIds);

        return Ok(comparisons);
    }
}