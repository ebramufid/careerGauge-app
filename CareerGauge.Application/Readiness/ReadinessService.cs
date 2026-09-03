using CareerGauge.Application.Recommendations.Dtos;
using CareerGauge.Application.Readiness.Dtos;
using CareerGauge.Domain.Entities;

namespace CareerGauge.Application.Readiness;

public class ReadinessService : IReadinessService
{
    private readonly IReadinessRepository _repository;

    public ReadinessService(
        IReadinessRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReadinessResultDto> CalculateAsync(
        int learnerId,
        int careerProfileId)
    {
        var learner = await _repository
            .GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with ID {learnerId} was not found.");
        }

        var career = await _repository
            .GetCareerProfileAsync(careerProfileId);

        if (career is null)
        {
            throw new KeyNotFoundException(
                $"Career profile with ID {careerProfileId} was not found.");
        }

        return CalculateReadiness(learner, career);
    }

    private static ReadinessResultDto CalculateReadiness(
        Learner learner,
        CareerProfile career)
    {
        var skillGaps = new List<SkillGapDto>();

        foreach (var requirement in career.SkillRequirements)
        {
            var learnerSkill = learner.LearnerSkills
                .FirstOrDefault(
                    ls => ls.SkillId == requirement.SkillId);

            var currentLevel = learnerSkill?.CurrentLevel ?? 0;
            var requiredLevel = requirement.RequiredLevel;

            var status = currentLevel switch
            {
                0 => "Missing",
                _ when currentLevel >= requiredLevel => "Met",
                _ => "Partial"
            };

            var gap = Math.Max(
                requiredLevel - currentLevel,
                0);

            skillGaps.Add(new SkillGapDto
            {
                SkillId = requirement.SkillId,
                SkillName = requirement.Skill.Name,
                CurrentLevel = currentLevel,
                RequiredLevel = requiredLevel,
                Gap = gap,
                Status = status
            });
        }

        var requiredSkills = skillGaps.Count;

        var metSkills = skillGaps.Count(
            sg => sg.Status == "Met");

        var partialSkills = skillGaps.Count(
            sg => sg.Status == "Partial");

        var missingSkills = skillGaps.Count(
            sg => sg.Status == "Missing");

        var readinessPercentage = requiredSkills == 0
            ? 0
            : ((metSkills + partialSkills * 0.5m)
                / requiredSkills) * 100m;

        var learningPriorities = skillGaps
    .Where(gap => gap.Gap > 0)
    .OrderByDescending(gap => gap.Status == "Missing")
    .ThenByDescending(gap => gap.Gap)
    .Select(gap => new LearningPriorityDto
    {
        SkillId = gap.SkillId,
        SkillName = gap.SkillName,
        CurrentLevel = gap.CurrentLevel,
        TargetLevel = gap.RequiredLevel,
        Gap = gap.Gap,
        Status = gap.Status,
        Priority = gap.Status == "Missing"
            ? "High"
            : "Medium"
    })
    .ToList();

        return new ReadinessResultDto
        {
            CareerProfileId = career.Id,
            CareerName = career.Name,
            ReadinessPercentage = Math.Round(readinessPercentage, 2),
            RequiredSkills = requiredSkills,
            MetSkills = metSkills,
            PartialSkills = partialSkills,
            MissingSkills = missingSkills,
            SkillGaps = skillGaps,
            LearningPriorities = learningPriorities
        };
    }
}