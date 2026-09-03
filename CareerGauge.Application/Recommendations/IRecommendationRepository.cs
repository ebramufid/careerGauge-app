using CareerGauge.Domain.Entities;

namespace CareerGauge.Application.Recommendations;

public interface IRecommendationRepository
{
    Task<Learner?> GetLearnerAsync(int learnerId);

    Task<List<CareerProfile>> GetCareerProfilesAsync();
}