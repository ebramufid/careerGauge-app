using CareerGauge.Application.Readiness;
using CareerGauge.Domain.Entities;
using CareerGauge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.Readiness;

public class ReadinessRepository : IReadinessRepository
{
    private readonly CareerGaugeDbContext _context;

    public ReadinessRepository(
        CareerGaugeDbContext context)
    {
        _context = context;
    }

    public async Task<Learner?> GetLearnerAsync(
    int learnerId)
    {
        return await _context.Learners
            .AsNoTracking()
            .Include(l => l.LearnerSkills)
            .ThenInclude(ls => ls.Skill)
            .FirstOrDefaultAsync(
                l => l.Id == learnerId);
    }

    public async Task<CareerProfile?> GetCareerProfileAsync(
        int careerProfileId)
    {
        return await _context.CareerProfiles
            .Include(cp => cp.SkillRequirements)
            .ThenInclude(csr => csr.Skill)
            .FirstOrDefaultAsync(
                cp => cp.Id == careerProfileId);
    }
}