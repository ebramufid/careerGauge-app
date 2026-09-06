using CareerGauge.Application.Recommendations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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


        if (!IsCurrentLearner(learnerId))
        {
            return Forbid();
        }

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
        if (!IsCurrentLearner(learnerId))
        {
            return Forbid();
        }

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
        if (!IsCurrentLearner(learnerId))
        {
            return Forbid();
        }

        var comparisons =
            await _recommendationService
                .GetComparisonAsync(
                    learnerId,
                    careerProfileIds);

        return Ok(comparisons);
    }

    [HttpGet("careers")]
    public async Task<IActionResult> GetCareerProfiles()
    {
        var careers =
            await _recommendationService
                .GetCareerProfilesAsync();

        return Ok(careers);
    }

    private bool IsCurrentLearner(int learnerId)
    {
        var authenticatedLearnerId =
            User.FindFirstValue("learnerId");

        return authenticatedLearnerId is not null
            && int.TryParse(
                authenticatedLearnerId,
                out var currentLearnerId)
            && currentLearnerId == learnerId;
    }
}