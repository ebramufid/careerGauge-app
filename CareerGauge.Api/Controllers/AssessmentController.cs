using System.Security.Claims;
using CareerGauge.Application.DTOs;
using CareerGauge.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssessmentController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public AssessmentController(
        IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet("{skillId:int}")]
    public async Task<IActionResult> GetQuestions(int skillId)
    {
        var questions = await _assessmentService
            .GetQuestionsAsync(skillId);

        if (questions.Count == 0)
        {
            return NotFound(new
            {
                message = "No assessment questions were found for this skill."
            });
        }

        return Ok(questions);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAssessment(
        [FromBody] SubmitAssessmentRequest request)
    {
        var learnerIdClaim = User.FindFirstValue("learnerId");

        if (learnerIdClaim is null ||
            !int.TryParse(learnerIdClaim, out var learnerId))
        {
            return Unauthorized(new
            {
                message = "Authenticated learner could not be identified."
            });
        }

        var result = await _assessmentService
            .SubmitAssessmentAsync(learnerId, request);

        if (result is null)
        {
            return BadRequest(new
            {
                message =
                    "The assessment submission is invalid or the learner does not have this skill."
            });
        }

        return Ok(result);
    }


    [HttpGet("history/{skillId:int}")]
    public async Task<IActionResult> GetLatestResult(int skillId)
    {
        var learnerIdClaim = User.FindFirstValue("learnerId");

        if (learnerIdClaim is null ||
            !int.TryParse(learnerIdClaim, out var learnerId))
        {
            return Unauthorized(new
            {
                message = "Authenticated learner could not be identified."
            });
        }

        var result = await _assessmentService
            .GetLatestResultAsync(learnerId, skillId);

        if (result is null)
        {
            return NotFound(new
            {
                message = "No assessment has been completed for this skill."
            });
        }

        return Ok(result);
    }
}