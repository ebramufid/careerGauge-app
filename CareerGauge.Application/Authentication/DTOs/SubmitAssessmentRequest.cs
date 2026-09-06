namespace CareerGauge.Application.DTOs;

public class SubmitAssessmentRequest
{
    public int SkillId { get; set; }

    public List<AssessmentAnswerDto> Answers { get; set; } = [];
}

public class AssessmentAnswerDto
{
    public int QuestionId { get; set; }

    public required string Answer { get; set; }
}