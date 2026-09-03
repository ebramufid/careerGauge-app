using CareerGauge.Application.Recommendations.Dtos;

namespace CareerGauge.Application.Readiness.Dtos;

public class ReadinessResultDto
{
    public int CareerProfileId { get; set; }
    public required string CareerName { get; set; }
    public decimal ReadinessPercentage { get; set; }
    public int RequiredSkills { get; set; }
    public int MetSkills { get; set; }
    public int PartialSkills { get; set; }
    public int MissingSkills { get; set; }
    public List<SkillGapDto> SkillGaps { get; set; } = [];
    public List<LearningPriorityDto> LearningPriorities { get; set; } = [];
}