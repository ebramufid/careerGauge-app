namespace CareerGauge.Application.LearnerSkills.Dtos;

public class LearnerSkillDto
{
    public int SkillId { get; set; }

    public required string SkillName { get; set; }

    public int CurrentLevel { get; set; }
}