using CareerGauge.Application.LearnerSkills;
using CareerGauge.Domain.Entities;
using CareerGauge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.LearnerSkills;

public class LearnerSkillRepository : ILearnerSkillRepository
{
    private readonly CareerGaugeDbContext _context;

    public LearnerSkillRepository(CareerGaugeDbContext context)
    {
        _context = context;
    }

    public async Task<Learner?> GetLearnerAsync(int learnerId)
    {
        return await _context.Learners
            .Include(l => l.LearnerSkills)
            .ThenInclude(ls => ls.Skill)
            .FirstOrDefaultAsync(l => l.Id == learnerId);
    }

    public async Task<List<Skill>> GetSkillsAsync()
    {
        return await _context.Skills
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<LearnerSkill?> GetLearnerSkillAsync(
        int learnerId,
        int skillId)
    {
        return await _context.LearnerSkills
            .FirstOrDefaultAsync(ls =>
                ls.LearnerId == learnerId &&
                ls.SkillId == skillId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}