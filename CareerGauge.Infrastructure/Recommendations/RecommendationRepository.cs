using CareerGauge.Application.Recommendations;
using CareerGauge.Domain.Entities;
using CareerGauge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.Recommendations;

public class RecommendationRepository
    : IRecommendationRepository
{
    private readonly CareerGaugeDbContext _context;

    public RecommendationRepository(
        CareerGaugeDbContext context)
    {
        _context = context;
    }

    public async Task<Learner?> GetLearnerAsync(
        int learnerId)
    {
        return await _context.Learners
            .Include(l => l.LearnerSkills)
            .ThenInclude(ls => ls.Skill)
            .FirstOrDefaultAsync(
                l => l.Id == learnerId);
    }

    public async Task<List<CareerProfile>>
        GetCareerProfilesAsync()
    {
        return await _context.CareerProfiles
            .Include(cp => cp.SkillRequirements)
            .ThenInclude(csr => csr.Skill)
            .ToListAsync();
    }
}