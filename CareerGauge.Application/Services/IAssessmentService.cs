using CareerGauge.Application.DTOs;

namespace CareerGauge.Application.Services;

public interface IAssessmentService
{
    Task<List<AssessmentQuestionDto>> GetQuestionsAsync(
        int skillId);

    Task<AssessmentResultDto?> SubmitAssessmentAsync(
        int learnerId,
        SubmitAssessmentRequest request);

    Task<AssessmentResultDto?> GetLatestResultAsync(
        int learnerId,
        int skillId);
}