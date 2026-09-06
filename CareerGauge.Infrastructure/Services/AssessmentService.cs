using CareerGauge.Application.DTOs;
using CareerGauge.Application.Services;
using CareerGauge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.Services;

public class AssessmentService : IAssessmentService
{
    private readonly CareerGaugeDbContext _context;

    public AssessmentService(CareerGaugeDbContext context)
    {
        _context = context;
    }

    public async Task<List<AssessmentQuestionDto>> GetQuestionsAsync(
        int skillId)
    {
        return await _context.AssessmentQuestions
            .AsNoTracking()
            .Where(q => q.SkillId == skillId)
            .OrderBy(q => q.Id)
            .Select(q => new AssessmentQuestionDto
            {
                Id = q.Id,
                SkillId = q.SkillId,
                QuestionText = q.QuestionText,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                Difficulty = q.Difficulty
            })
            .ToListAsync();
    }

    public async Task<AssessmentResultDto?> SubmitAssessmentAsync(
        int learnerId,
        SubmitAssessmentRequest request)
    {
        if (request.Answers.Count == 0)
        {
            return null;
        }

        var questions = await _context.AssessmentQuestions
            .Where(q => q.SkillId == request.SkillId)
            .ToListAsync();

        if (questions.Count == 0)
        {
            return null;
        }

        var questionIds = questions
            .Select(q => q.Id)
            .ToHashSet();

        var submittedQuestionIds = request.Answers
            .Select(a => a.QuestionId)
            .ToHashSet();

        if (!submittedQuestionIds.IsSubsetOf(questionIds))
        {
            return null;
        }

        var correctAnswers = questions.ToDictionary(
            q => q.Id,
            q => q.CorrectAnswer);

        var score = 0;

        foreach (var answer in request.Answers)
        {
            if (!correctAnswers.TryGetValue(
                    answer.QuestionId,
                    out var correctAnswer))
            {
                continue;
            }

            if (string.Equals(
                    answer.Answer.Trim(),
                    correctAnswer,
                    StringComparison.OrdinalIgnoreCase))
            {
                score++;
            }
        }

        var totalQuestions = questions.Count;

        var percentage = (int)Math.Round(
            score * 100.0 / totalQuestions);

        var resultLevel = DetermineLevel(percentage);

        var learnerSkill = await _context.LearnerSkills
            .SingleOrDefaultAsync(ls =>
                ls.LearnerId == learnerId &&
                ls.SkillId == request.SkillId);

        if (learnerSkill is null)
        {
            return null;
        }

        learnerSkill.CurrentLevel = resultLevel;

        var attempt = new Domain.Entities.AssessmentAttempt
        {
            LearnerId = learnerId,
            SkillId = request.SkillId,
            Score = score,
            ResultLevel = resultLevel,
            CompletedAt = DateTime.UtcNow
        };

        await _context.AssessmentAttempts.AddAsync(attempt);

        await _context.SaveChangesAsync();

        return new AssessmentResultDto
        {
            SkillId = request.SkillId,
            Score = score,
            TotalQuestions = totalQuestions,
            Percentage = percentage,
            ResultLevel = resultLevel,
            LevelName = GetLevelName(resultLevel)
        };
    }

    private static int DetermineLevel(int percentage)
    {
        return percentage switch
        {
            < 40 => 1,
            < 70 => 2,
            _ => 3
        };
    }

    private static string GetLevelName(int level)
    {
        return level switch
        {
            1 => "Beginner",
            2 => "Intermediate",
            3 => "Advanced",
            _ => "Unknown"
        };
    }
}