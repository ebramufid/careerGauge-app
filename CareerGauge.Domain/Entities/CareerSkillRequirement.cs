namespace CareerGauge.Domain.Entities;

public class CareerSkillRequirement
{
    public int Id { get; set; }

    public int CareerProfileId { get; set; }

    public int SkillId { get; set; }

    // Minimum proficiency required for this career.
    public int RequiredLevel { get; set; }

    public CareerProfile CareerProfile { get; set; } = null!;

    public Skill Skill { get; set; } = null!;
}