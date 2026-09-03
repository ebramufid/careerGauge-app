namespace CareerGauge.Domain.Entities;

public class LearnerSkill
{
    public int Id { get; set; }

    public int LearnerId { get; set; }

    public int SkillId { get; set; }

    // 0 = Missing
    // 1 = Beginner
    // 2 = Intermediate
    // 3 = Advanced
    public int CurrentLevel { get; set; }

    public Learner Learner { get; set; } = null!;

    public Skill Skill { get; set; } = null!;
}