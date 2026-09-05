using Microsoft.AspNetCore.Authorization;
using CareerGauge.Application.LearnerSkills;
using CareerGauge.Application.LearnerSkills.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearnerSkillsController : ControllerBase
{
    private readonly ILearnerSkillService _learnerSkillService;

    public LearnerSkillsController(
        ILearnerSkillService learnerSkillService)
    {
        _learnerSkillService = learnerSkillService;
    }

    [HttpGet("{learnerId}")]
    public async Task<IActionResult> GetLearnerSkills(
        int learnerId)
    {
        var authenticatedLearnerId =
            User.FindFirstValue("learnerId");

        if (authenticatedLearnerId is null ||
            !int.TryParse(
                authenticatedLearnerId,
                out var currentLearnerId))
        {
            return Unauthorized();
        }

        if (currentLearnerId != learnerId)
        {
            return Forbid();
        }

        var skills =
            await _learnerSkillService
                .GetLearnerSkillsAsync(learnerId);

        return Ok(skills);
    }

    [HttpPut("{learnerId}")]
    public async Task<IActionResult> UpdateLearnerSkills(
        int learnerId,
        [FromBody] List<UpdateLearnerSkillDto> updates)
    {
        var authenticatedLearnerId =
            User.FindFirstValue("learnerId");

        if (authenticatedLearnerId is null ||
            !int.TryParse(
                authenticatedLearnerId,
                out var currentLearnerId))
        {
            return Unauthorized();
        }

        if (currentLearnerId != learnerId)
        {
            return Forbid();
        }

        var skills =
            await _learnerSkillService
                .UpdateLearnerSkillsAsync(
                    learnerId,
                    updates);

        return Ok(skills);
    }
}