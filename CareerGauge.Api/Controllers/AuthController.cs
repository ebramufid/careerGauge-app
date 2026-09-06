using System.Security.Claims;
using CareerGauge.Application.Authentication;
using CareerGauge.Application.Authentication.DTOs;
using CareerGauge.Domain.Entities;
using CareerGauge.Infrastructure.Identity;
using CareerGauge.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<CareerGaugeUser> _userManager;
    private readonly CareerGaugeDbContext _dbContext;

    public AuthController(
        IAuthService authService,
        UserManager<CareerGaugeUser> userManager,
        CareerGaugeDbContext dbContext)
    {
        _authService = authService;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return Conflict(new
            {
                message = "An account with this email already exists."
            });
        }

        var learner = new Learner
        {
            Name = request.Name,
            Email = request.Email
        };

        _dbContext.Learners.Add(learner);

        await _dbContext.SaveChangesAsync();

        var user = new CareerGaugeUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            LearnerId = learner.Id
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            _dbContext.Learners.Remove(learner);
            await _dbContext.SaveChangesAsync();

            var errors = result.Errors
                .Select(error => error.Description)
                .ToList();

            return BadRequest(new
            {
                message = "Registration failed.",
                errors
            });
        }

        return Ok(new
        {
            message = "Registration successful."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        Response.Cookies.Append(
            "accessToken",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

        return Ok(new
        {
            learnerId = result.LearnerId,
            email = result.Email
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var learnerId = User.FindFirstValue("learnerId");

        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        if (learnerId is null || email is null)
        {
            return Unauthorized();
        }

        return Ok(new CurrentUserResponse
        {
            LearnerId = int.Parse(learnerId),
            Email = email
        });
    }
}