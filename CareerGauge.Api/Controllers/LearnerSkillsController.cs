using Microsoft.AspNetCore.Authorization;
using CareerGauge.Application.LearnerSkills;
using CareerGauge.Application.LearnerSkills.Dtos;
using Microsoft.AspNetCore.Mvc;

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
        var skills =
            await _learnerSkillService
                .UpdateLearnerSkillsAsync(
                    learnerId,
                    updates);

        return Ok(skills);
    }
}