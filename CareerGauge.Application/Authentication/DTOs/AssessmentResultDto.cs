namespace CareerGauge.Application.DTOs;

public class AssessmentResultDto
{
    public int SkillId { get; set; }

    public int Score { get; set; }

    public int TotalQuestions { get; set; }

    public int Percentage { get; set; }

    public int ResultLevel { get; set; }

    public required string LevelName { get; set; }
}