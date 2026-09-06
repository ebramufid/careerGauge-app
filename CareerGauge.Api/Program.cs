using Microsoft.AspNetCore.Authentication.JwtBearer;
using CareerGauge.Application.Authentication;
using CareerGauge.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using CareerGauge.Application.LearnerSkills;
using CareerGauge.Application.Readiness;
using CareerGauge.Application.Recommendations;
using CareerGauge.Infrastructure.LearnerSkills;
using CareerGauge.Infrastructure.Persistence;
using CareerGauge.Infrastructure.Readiness;
using CareerGauge.Infrastructure.Recommendations;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CareerGauge.Application.Services;
using CareerGauge.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<CareerGaugeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("CareerGaugeDatabase")));

builder.Services.AddDbContext<CareerGaugeIdentityDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("CareerGaugeDatabase")));

builder.Services
    .AddIdentityCore<CareerGaugeUser>()
    .AddEntityFrameworkStores<CareerGaugeIdentityDbContext>()
    .AddSignInManager();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];

                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["Key"]
                    ?? throw new InvalidOperationException(
                        "JWT key is not configured.")))
        };
    });

builder.Services.AddAuthorization();
// Application services
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


builder.Services.AddScoped<IReadinessService, ReadinessService>();
builder.Services.AddScoped<IReadinessRepository, ReadinessRepository>();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();

builder.Services.AddScoped<ILearnerSkillRepository, LearnerSkillRepository>();
builder.Services.AddScoped<ILearnerSkillService, LearnerSkillService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4201")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed development data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services
        .GetRequiredService<CareerGaugeDbContext>();

    await DataSeeder.SeedAsync(context);

    await DataSeeder.SeedIdentityUserAsync(services);
}

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();