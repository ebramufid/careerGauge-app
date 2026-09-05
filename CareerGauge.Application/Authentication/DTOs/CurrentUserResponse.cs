namespace CareerGauge.Application.Authentication.DTOs;

public class CurrentUserResponse
{
    public int LearnerId { get; set; }
    public required string Email { get; set; }
}