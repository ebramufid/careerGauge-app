using CareerGauge.Application.Recommendations.Dtos;

namespace CareerGauge.Application.Recommendations;

public interface IRecommendationService
{
    Task<List<CareerRecommendationDto>> GetRecommendationsAsync(
        int learnerId);
}