using CareerGauge.Application.Authentication.DTOs;
using System.Security.Claims;
using CareerGauge.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CareerGauge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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