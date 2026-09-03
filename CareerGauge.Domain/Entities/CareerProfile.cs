namespace CareerGauge.Domain.Entities;

public class CareerProfile
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<CareerSkillRequirement> SkillRequirements { get; set; } = [];
}