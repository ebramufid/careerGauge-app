using CareerGauge.Application.LearnerSkills.Dtos;

namespace CareerGauge.Application.LearnerSkills;

public interface ILearnerSkillService
{
    Task<List<LearnerSkillDto>> GetLearnerSkillsAsync(
        int learnerId);

    Task<List<LearnerSkillDto>> UpdateLearnerSkillsAsync(
        int learnerId,
        List<UpdateLearnerSkillDto> updates);
}