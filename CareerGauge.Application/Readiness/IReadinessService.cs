using CareerGauge.Application.Readiness.Dtos;

namespace CareerGauge.Application.Readiness;

public interface IReadinessService
{
    Task<ReadinessResultDto> CalculateAsync(
        int learnerId,
        int careerProfileId);
}