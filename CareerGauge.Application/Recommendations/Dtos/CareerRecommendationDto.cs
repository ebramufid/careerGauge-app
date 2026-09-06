using CareerGauge.Application.Readiness.Dtos;

namespace CareerGauge.Application.Recommendations.Dtos;

public class CareerRecommendationDto
{
    public int CareerProfileId { get; set; }

    public required string CareerName { get; set; }

    public decimal ReadinessPercentage { get; set; }

    public int RequiredSkills { get; set; }

    public int MetSkills { get; set; }

    public int PartialSkills { get; set; }

    public int MissingSkills { get; set; }

    public int SkillGapCount { get; set; }

    public List<string> Strengths { get; set; } = [];

    public List<SkillGapDto> SkillGaps { get; set; } = [];

    public List<LearningPriorityDto> LearningPriorities { get; set; } = [];
}