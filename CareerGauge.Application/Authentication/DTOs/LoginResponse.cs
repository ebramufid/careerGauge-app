namespace CareerGauge.Application.Authentication.DTOs;

public class LoginResponse
{
    public required string AccessToken { get; set; }

    public int LearnerId { get; set; }

    public required string Email { get; set; }
}