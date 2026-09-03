namespace CareerGauge.Application.Readiness.Dtos;

public class SkillGapDto
{
    public int SkillId { get; set; }

    public required string SkillName { get; set; }

    public int CurrentLevel { get; set; }

    public int RequiredLevel { get; set; }

    public int Gap { get; set; }

    public required string Status { get; set; }
}