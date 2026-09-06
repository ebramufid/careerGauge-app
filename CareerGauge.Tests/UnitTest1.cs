using CareerGauge.Application.DTOs;
using CareerGauge.Domain.Entities;
using CareerGauge.Infrastructure.Persistence;
using CareerGauge.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Tests;

public class AssessmentServiceTests
{
    [Fact]
    public async Task SubmitAssessmentAsync_WithCorrectAnswers_ReturnsAdvanced()
    {
        var options = new DbContextOptionsBuilder<CareerGaugeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new CareerGaugeDbContext(options);

        var skill = new Skill
        {
            Id = 1,
            Name = "C#"
        };

        var learner = new Learner
        {
            Id = 1,
            Name = "Test Learner"
        };

        context.Skills.Add(skill);
        context.Learners.Add(learner);

        context.AssessmentQuestions.AddRange(
            new AssessmentQuestion
            {
                Id = 1,
                SkillId = 1,
                QuestionText = "Question 1",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "A",
                Difficulty = 1
            },
            new AssessmentQuestion
            {
                Id = 2,
                SkillId = 1,
                QuestionText = "Question 2",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "B",
                Difficulty = 1
            },
            new AssessmentQuestion
            {
                Id = 3,
                SkillId = 1,
                QuestionText = "Question 3",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "C",
                Difficulty = 1
            },
            new AssessmentQuestion
            {
                Id = 4,
                SkillId = 1,
                QuestionText = "Question 4",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "D",
                Difficulty = 2
            },
            new AssessmentQuestion
            {
                Id = 5,
                SkillId = 1,
                QuestionText = "Question 5",
                OptionA = "A",
                OptionB = "B",
                OptionC = "C",
                OptionD = "D",
                CorrectAnswer = "A",
                Difficulty = 2
            });

        await context.SaveChangesAsync();

        var service = new AssessmentService(context);

        var request = new SubmitAssessmentRequest
        {
            SkillId = 1,
            Answers =
            [
          new AssessmentAnswerDto { QuestionId = 1, Answer = "A" },
          new AssessmentAnswerDto { QuestionId = 2, Answer = "B" },
          new AssessmentAnswerDto { QuestionId = 3, Answer = "C" },
          new AssessmentAnswerDto { QuestionId = 4, Answer = "D" },
          new AssessmentAnswerDto { QuestionId = 5, Answer = "A"  }
            ]
        };

        var result = await service.SubmitAssessmentAsync(1, request);

        Assert.NotNull(result);
        Assert.Equal(5, result.Score);
        Assert.Equal(5, result.TotalQuestions);
        Assert.Equal(100, result.Percentage);
        Assert.Equal(3, result.ResultLevel);
        Assert.Equal("Advanced", result.LevelName);

        var learnerSkill = await context.LearnerSkills
            .SingleAsync(ls =>
                ls.LearnerId == 1 &&
                ls.SkillId == 1);

        Assert.Equal(3, learnerSkill.CurrentLevel);

        var attempt = await context.AssessmentAttempts
            .SingleAsync(a =>
                a.LearnerId == 1 &&
                a.SkillId == 1);

        Assert.Equal(5, attempt.Score);
        Assert.Equal(3, attempt.ResultLevel);
    }
}