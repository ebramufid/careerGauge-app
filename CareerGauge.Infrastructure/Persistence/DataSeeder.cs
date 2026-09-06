using CareerGauge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CareerGauge.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CareerGauge.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedIdentityUserAsync(
    IServiceProvider services)
    {
        var userManager = services.GetRequiredService<
            UserManager<CareerGaugeUser>>();

        var dbContext = services.GetRequiredService<
            CareerGaugeDbContext>();

        var existingUser = await userManager.FindByEmailAsync(
            "demo@careergauge.local");

        if (existingUser is not null)
        {
            return;
        }

        var learner = await dbContext.Learners
            .SingleOrDefaultAsync(
                l => l.Email == "demo@careergauge.local");

        if (learner is null)
        {
            throw new InvalidOperationException(
                "Demo learner was not found before creating the Identity user.");
        }

        var user = new CareerGaugeUser
        {
            UserName = "demo@careergauge.local",
            Email = "demo@careergauge.local",
            EmailConfirmed = true,
            LearnerId = learner.Id
        };

        var result = await userManager.CreateAsync(
            user,
            "Demo123!");

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(
                $"Failed to create demo Identity user: {errors}");
        }
    }

    //////////////////////////////////////////////////////////////////
    public static async Task SeedAsync(CareerGaugeDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedSkillsAsync(context);
        await SeedAssessmentQuestionsAsync(context);
        await SeedCareersAsync(context);
        await SeedLearnersAsync(context);
    }

    private static async Task SeedSkillsAsync(
        CareerGaugeDbContext context)
    {
        if (await context.Skills.AnyAsync())
            return;

        var skills = new[]
        {
            new Skill
            {
                Name = "C#",
                Description = "Programming with the C# language."
            },
            new Skill
            {
                Name = "ASP.NET Core",
                Description = "Building web applications and APIs with ASP.NET Core."
            },
            new Skill
            {
                Name = "Entity Framework Core",
                Description = "Object-relational mapping with EF Core."
            },
            new Skill
            {
                Name = "REST API Development",
                Description = "Designing and developing RESTful APIs."
            },
            new Skill
            {
                Name = "SQL",
                Description = "Working with relational databases using SQL."
            },
            new Skill
            {
                Name = "PostgreSQL",
                Description = "Developing applications using PostgreSQL."
            },
            new Skill
            {
                Name = "Git",
                Description = "Version control and collaborative development with Git."
            },
            new Skill
            {
                Name = "Automated Testing",
                Description = "Writing and maintaining automated software tests."
            },
            new Skill
            {
                Name = "Web Security",
                Description = "Applying security principles to web applications and APIs."
            },
            new Skill
            {
                Name = "TypeScript",
                Description = "Typed JavaScript development with TypeScript."
            },
            new Skill
            {
                Name = "Angular",
                Description = "Building modern web applications with Angular."
            },
            new Skill
            {
                Name = "HTML/CSS",
                Description = "Building and styling web interfaces."
            },
            new Skill
            {
                Name = "Docker",
                Description = "Containerization and application deployment."
            },
            new Skill
            {
                Name = "CI/CD",
                Description = "Continuous integration and continuous delivery practices."
            },
            new Skill
            {
                Name = "Linux",
                Description = "Linux operating system and command-line skills."
            },
            new Skill
            {
                Name = "Python",
                Description = "Programming with Python."
            },
            new Skill
            {
                Name = "Data Analysis",
                Description = "Analyzing and interpreting datasets."
            },
            new Skill
            {
                Name = "Statistics",
                Description = "Statistical analysis and reasoning."
            }
        };

        await context.Skills.AddRangeAsync(skills);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAssessmentQuestionsAsync(
    CareerGaugeDbContext context)
    {
        var csharpSkill = await context.Skills
            .FirstOrDefaultAsync(s => s.Name == "C#");

        if (csharpSkill is null)
        {
            return;
        }

        var questionsAlreadyExist = await context.AssessmentQuestions
            .AnyAsync(q => q.SkillId == csharpSkill.Id);

        if (questionsAlreadyExist)
        {
            return;
        }

        var questions = new List<AssessmentQuestion>
    {
        new()
        {
            SkillId = csharpSkill.Id,
            QuestionText =
                "Which keyword is used to create a new instance of a class in C#?",
            OptionA = "class",
            OptionB = "new",
            OptionC = "create",
            OptionD = "instance",
            CorrectAnswer = "B",
            Difficulty = 1
        },

        new()
        {
            SkillId = csharpSkill.Id,
            QuestionText =
                "What is the output of: int x = 5; Console.WriteLine(x++);",
            OptionA = "4",
            OptionB = "5",
            OptionC = "6",
            OptionD = "Compilation error",
            CorrectAnswer = "B",
            Difficulty = 1
        },

        new()
        {
            SkillId = csharpSkill.Id,
            QuestionText =
                "Which C# type is used to represent a value that can be either true or false?",
            OptionA = "bool",
            OptionB = "int",
            OptionC = "string",
            OptionD = "double",
            CorrectAnswer = "A",
            Difficulty = 1
        },

        new()
        {
            SkillId = csharpSkill.Id,
            QuestionText =
                "Which feature allows a C# class to provide multiple implementations of a method with the same name but different parameters?",
            OptionA = "Inheritance",
            OptionB = "Encapsulation",
            OptionC = "Method overloading",
            OptionD = "Abstraction",
            CorrectAnswer = "C",
            Difficulty = 2
        },

        new()
        {
            SkillId = csharpSkill.Id,
            QuestionText =
                "What does the async keyword primarily enable in a C# method?",
            OptionA = "The method can execute asynchronous operations using await",
            OptionB = "The method always runs on a new thread",
            OptionC = "The method cannot return a value",
            OptionD = "The method becomes static",
            CorrectAnswer = "A",
            Difficulty = 2
        }
    };

        await context.AssessmentQuestions.AddRangeAsync(questions);

        await context.SaveChangesAsync();
    }

    private static async Task SeedCareersAsync(
        CareerGaugeDbContext context)
    {
        if (await context.CareerProfiles.AnyAsync())
            return;

        var skills = await context.Skills
            .ToDictionaryAsync(s => s.Name, StringComparer.OrdinalIgnoreCase);

        var careers = new[]
        {
            new CareerProfile
            {
                Name = "Backend Developer",
                Description =
                    "Builds server-side applications, APIs, and data-driven systems.",
                SkillRequirements =
                [
                    Requirement(skills, "C#", 2),
                    Requirement(skills, "ASP.NET Core", 2),
                    Requirement(skills, "Entity Framework Core", 2),
                    Requirement(skills, "REST API Development", 2),
                    Requirement(skills, "SQL", 2),
                    Requirement(skills, "PostgreSQL", 1),
                    Requirement(skills, "Git", 2),
                    Requirement(skills, "Automated Testing", 1),
                    Requirement(skills, "Web Security", 2)
                ]
            },

            new CareerProfile
            {
                Name = "Full-Stack Developer",
                Description =
                    "Develops both frontend and backend parts of modern web applications.",
                SkillRequirements =
                [
                    Requirement(skills, "C#", 2),
                    Requirement(skills, "ASP.NET Core", 2),
                    Requirement(skills, "REST API Development", 2),
                    Requirement(skills, "SQL", 2),
                    Requirement(skills, "Git", 2),
                    Requirement(skills, "TypeScript", 2),
                    Requirement(skills, "Angular", 2),
                    Requirement(skills, "HTML/CSS", 2),
                    Requirement(skills, "Web Security", 1),
                    Requirement(skills, "Automated Testing", 1)
                ]
            },

            new CareerProfile
            {
                Name = "Frontend Developer",
                Description =
                    "Builds responsive and interactive user interfaces for web applications.",
                SkillRequirements =
                [
                    Requirement(skills, "TypeScript", 2),
                    Requirement(skills, "Angular", 2),
                    Requirement(skills, "HTML/CSS", 2),
                    Requirement(skills, "Git", 2),
                    Requirement(skills, "REST API Development", 1),
                    Requirement(skills, "Automated Testing", 1),
                    Requirement(skills, "Web Security", 1)
                ]
            },

            new CareerProfile
            {
                Name = "DevOps Engineer",
                Description =
                    "Automates software delivery, infrastructure, deployment, and operations.",
                SkillRequirements =
                [
                    Requirement(skills, "Linux", 2),
                    Requirement(skills, "Docker", 2),
                    Requirement(skills, "CI/CD", 2),
                    Requirement(skills, "Git", 2),
                    Requirement(skills, "SQL", 1),
                    Requirement(skills, "Web Security", 2),
                    Requirement(skills, "Python", 1)
                ]
            },

            new CareerProfile
            {
                Name = "Data Analyst",
                Description =
                    "Uses data, statistics, and analytical tools to support decisions.",
                SkillRequirements =
                [
                    Requirement(skills, "Python", 2),
                    Requirement(skills, "SQL", 2),
                    Requirement(skills, "Data Analysis", 2),
                    Requirement(skills, "Statistics", 2),
                    Requirement(skills, "Git", 1)
                ]
            }
        };

        await context.CareerProfiles.AddRangeAsync(careers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLearnersAsync(
        CareerGaugeDbContext context)
    {
        if (await context.Learners.AnyAsync())
            return;

        var learner = new Learner
        {
            Name = "Demo Learner",
            Email = "demo@careergauge.local"
        };

        var skills = await context.Skills
            .ToDictionaryAsync(s => s.Name, StringComparer.OrdinalIgnoreCase);

        learner.LearnerSkills =
        [
            LearnerSkill(skills, "C#", 2),
            LearnerSkill(skills, "ASP.NET Core", 2),
            LearnerSkill(skills, "Entity Framework Core", 0),
            LearnerSkill(skills, "REST API Development", 2),
            LearnerSkill(skills, "SQL", 1),
            LearnerSkill(skills, "PostgreSQL", 0),
            LearnerSkill(skills, "Git", 2),
            LearnerSkill(skills, "Automated Testing", 0),
            LearnerSkill(skills, "Web Security", 0),
            LearnerSkill(skills, "TypeScript", 1),
            LearnerSkill(skills, "Angular", 1),
            LearnerSkill(skills, "HTML/CSS", 2),
            LearnerSkill(skills, "Docker", 0),
            LearnerSkill(skills, "CI/CD", 0),
            LearnerSkill(skills, "Linux", 0),
            LearnerSkill(skills, "Python", 0),
            LearnerSkill(skills, "Data Analysis", 0),
            LearnerSkill(skills, "Statistics", 0)
        ];

        await context.Learners.AddAsync(learner);
        await context.SaveChangesAsync();
    }

    private static CareerSkillRequirement Requirement(
        Dictionary<string, Skill> skills,
        string skillName,
        int requiredLevel)
    {
        return new CareerSkillRequirement
        {
            Skill = skills[skillName],
            RequiredLevel = requiredLevel
        };
    }

    private static LearnerSkill LearnerSkill(
        Dictionary<string, Skill> skills,
        string skillName,
        int currentLevel)
    {
        return new LearnerSkill
        {
            Skill = skills[skillName],
            CurrentLevel = currentLevel
        };
    }
}