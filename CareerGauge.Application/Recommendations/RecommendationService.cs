using CareerGauge.Application.Readiness;
using CareerGauge.Application.Recommendations.Dtos;
using CareerGauge.Domain.Entities;

namespace CareerGauge.Application.Recommendations;

public class RecommendationService : IRecommendationService
{
    private readonly IRecommendationRepository _repository;
    private readonly IReadinessService _readinessService;

    public RecommendationService(
        IRecommendationRepository repository,
        IReadinessService readinessService)
    {
        _repository = repository;
        _readinessService = readinessService;
    }

    public async Task<List<CareerRecommendationDto>>
        GetRecommendationsAsync(int learnerId)
    {
        var learner = await _repository
            .GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with ID {learnerId} was not found.");
        }

        var careers = await _repository
            .GetCareerProfilesAsync();

        var recommendations = new List<CareerRecommendationDto>();

        foreach (var career in careers)
        {
            var readiness =
                await _readinessService.CalculateAsync(
                    learnerId,
                    career.Id);

            recommendations.Add(new CareerRecommendationDto
            {
                CareerProfileId = career.Id,
                CareerName = career.Name,
                ReadinessPercentage =
                    readiness.ReadinessPercentage,
                RequiredSkills =
                    readiness.RequiredSkills,
                MetSkills =
                    readiness.MetSkills,
                PartialSkills =
                    readiness.PartialSkills,
                MissingSkills =
                    readiness.MissingSkills,
                SkillGapCount =
                    readiness.SkillGaps.Count(
                        gap => gap.Gap > 0)
            });
        }

        return recommendations
            .OrderByDescending(
                recommendation =>
                    recommendation.ReadinessPercentage)
            .ThenBy(
                recommendation =>
                    recommendation.SkillGapCount)
            .ToList();
    }
}