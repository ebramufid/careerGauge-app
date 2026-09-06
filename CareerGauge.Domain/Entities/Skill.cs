namespace CareerGauge.Domain.Entities;

public class Skill
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<LearnerSkill> LearnerSkills { get; set; } = [];

    public ICollection<CareerSkillRequirement> CareerSkillRequirements { get; set; } = [];

    public ICollection<AssessmentQuestion> AssessmentQuestions { get; set; } = [];

    public ICollection<AssessmentAttempt> AssessmentAttempts { get; set; } = [];
}