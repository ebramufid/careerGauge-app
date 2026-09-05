using CareerGauge.Application.LearnerSkills;
using CareerGauge.Application.Readiness;
using CareerGauge.Application.Recommendations;
using CareerGauge.Infrastructure.LearnerSkills;
using CareerGauge.Infrastructure.Persistence;
using CareerGauge.Infrastructure.Readiness;
using CareerGauge.Infrastructure.Recommendations;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<CareerGaugeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("CareerGaugeDatabase")));

// Application services
builder.Services.AddScoped<IReadinessService, ReadinessService>();
builder.Services.AddScoped<IReadinessRepository, ReadinessRepository>();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();

builder.Services.AddScoped<ILearnerSkillRepository, LearnerSkillRepository>();
builder.Services.AddScoped<ILearnerSkillService, LearnerSkillService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CareerGaugeClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:4201")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed development data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<CareerGaugeDbContext>();

    await DataSeeder.SeedAsync(context);
}

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("CareerGaugeClient");

app.MapControllers();

app.Run();