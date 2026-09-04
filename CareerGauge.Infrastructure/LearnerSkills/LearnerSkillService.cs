using CareerGauge.Application.LearnerSkills;
using CareerGauge.Application.LearnerSkills.Dtos;
using CareerGauge.Domain.Entities;

namespace CareerGauge.Infrastructure.LearnerSkills;

public class LearnerSkillService : ILearnerSkillService
{
    private readonly ILearnerSkillRepository _repository;

    public LearnerSkillService(
        ILearnerSkillRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LearnerSkillDto>> GetLearnerSkillsAsync(
        int learnerId)
    {
        var learner = await _repository.GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with id {learnerId} was not found.");
        }

        var skills = await _repository.GetSkillsAsync();

        return skills.Select(skill =>
        {
            var learnerSkill = learner.LearnerSkills
                .FirstOrDefault(ls => ls.SkillId == skill.Id);

            return new LearnerSkillDto
            {
                SkillId = skill.Id,
                SkillName = skill.Name,
                CurrentLevel = learnerSkill?.CurrentLevel ?? 0
            };
        }).ToList();
    }

    public async Task<List<LearnerSkillDto>> UpdateLearnerSkillsAsync(
        int learnerId,
        List<UpdateLearnerSkillDto> updates)
    {
        var learner = await _repository.GetLearnerAsync(learnerId);

        if (learner is null)
        {
            throw new KeyNotFoundException(
                $"Learner with id {learnerId} was not found.");
        }

        foreach (var update in updates)
        {
            if (update.CurrentLevel < 0 ||
                update.CurrentLevel > 3)
            {
                throw new ArgumentException(
                    "CurrentLevel must be between 0 and 3.");
            }

            var learnerSkill =
                await _repository.GetLearnerSkillAsync(
                    learnerId,
                    update.SkillId);

            if (learnerSkill is null)
            {
                learnerSkill = new LearnerSkill
                {
                    LearnerId = learnerId,
                    SkillId = update.SkillId,
                    CurrentLevel = update.CurrentLevel
                };

                learner.LearnerSkills.Add(learnerSkill);
            }
            else
            {
                learnerSkill.CurrentLevel =
                    update.CurrentLevel;
            }
        }

        await _repository.SaveChangesAsync();

        return await GetLearnerSkillsAsync(learnerId);
    }
}