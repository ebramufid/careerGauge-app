using CareerGauge.Domain.Entities;

namespace CareerGauge.Application.Readiness;

public interface IReadinessRepository
{
    Task<Learner?> GetLearnerAsync(int learnerId);

    Task<CareerProfile?> GetCareerProfileAsync(
        int careerProfileId);
}