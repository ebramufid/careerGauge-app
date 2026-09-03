namespace CareerGauge.Application.Recommendations.Dtos;

public class LearningPriorityDto
{
    public int SkillId { get; set; }
    public required string SkillName { get; set; }
    public int CurrentLevel { get; set; }
    public int TargetLevel { get; set; }
    public int Gap { get; set; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
}