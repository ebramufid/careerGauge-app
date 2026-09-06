namespace CareerGauge.Domain.Entities;

public class AssessmentQuestion
{
    public int Id { get; set; }

    public int SkillId { get; set; }

    public required string QuestionText { get; set; }

    public required string OptionA { get; set; }

    public required string OptionB { get; set; }

    public required string OptionC { get; set; }

    public required string OptionD { get; set; }

    public required string CorrectAnswer { get; set; }

    public int Difficulty { get; set; }

    public Skill Skill { get; set; } = null!;
}