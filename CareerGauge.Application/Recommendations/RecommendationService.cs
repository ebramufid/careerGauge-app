using CareerGauge.Application.Readiness;
using CareerGauge.Application.Readiness.Dtos;
using CareerGauge.Application.Recommendations.Dtos;

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
        var learner = await _repository.GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with ID {learnerId} was not found.");
        }

        var careers = await _repository.GetCareerProfilesAsync();

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
                ReadinessPercentage = readiness.ReadinessPercentage,
                RequiredSkills = readiness.RequiredSkills,
                MetSkills = readiness.MetSkills,
                PartialSkills = readiness.PartialSkills,
                MissingSkills = readiness.MissingSkills,
                SkillGapCount = readiness.SkillGaps.Count(
                    gap => gap.Gap > 0)
            });
        }

        return recommendations
            .OrderByDescending(r => r.ReadinessPercentage)
            .ThenBy(r => r.SkillGapCount)
            .ToList();
    }

    public async Task<ReadinessResultDto> GetRecommendationDetailsAsync(
        int learnerId,
        int careerProfileId)
    {
        var learner = await _repository.GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with ID {learnerId} was not found.");
        }

        return await _readinessService.CalculateAsync(
            learnerId,
            careerProfileId);
    }

    public async Task<List<CareerComparisonDto>> GetComparisonAsync(
    int learnerId,
    List<int> careerProfileIds)
    {
        if (careerProfileIds.Count < 2)
        {
            throw new ArgumentException(
                "At least two careers are required for comparison.");
        }

        if (careerProfileIds.Count > 3)
        {
            throw new ArgumentException(
                "A maximum of three careers can be compared.");
        }

        var learner = await _repository.GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with id {learnerId} was not found.");
        }

        var careerProfiles =
            await _repository.GetCareerProfilesAsync();

        var selectedCareers = careerProfiles
            .Where(c => careerProfileIds.Contains(c.Id))
            .ToList();

        if (selectedCareers.Count != careerProfileIds.Distinct().Count())
        {
            throw new KeyNotFoundException(
                "One or more selected careers were not found.");
        }

        var comparisons = new List<CareerComparisonDto>();

        foreach (var career in selectedCareers)
        {
            var readiness =
                await _readinessService.CalculateAsync(
                    learnerId,
                    career.Id);

            comparisons.Add(new CareerComparisonDto
            {
                CareerProfileId = readiness.CareerProfileId,
                CareerName = readiness.CareerName,
                ReadinessPercentage =
                    readiness.ReadinessPercentage,
                RequiredSkills = readiness.RequiredSkills,
                MetSkills = readiness.MetSkills,
                PartialSkills = readiness.PartialSkills,
                MissingSkills = readiness.MissingSkills,
                SkillGapCount =
                    readiness.SkillGaps.Count(g => g.Gap > 0)
            });
        }

        return comparisons
            .OrderByDescending(c => c.ReadinessPercentage)
            .ThenBy(c => c.SkillGapCount)
            .ToList();
    }
}