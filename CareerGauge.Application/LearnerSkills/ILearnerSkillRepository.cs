using CareerGauge.Domain.Entities;

namespace CareerGauge.Application.LearnerSkills;

public interface ILearnerSkillRepository
{
    Task<Learner?> GetLearnerAsync(int learnerId);

    Task<List<Skill>> GetSkillsAsync();

    Task<LearnerSkill?> GetLearnerSkillAsync(
        int learnerId,
        int skillId);

    Task SaveChangesAsync();
}