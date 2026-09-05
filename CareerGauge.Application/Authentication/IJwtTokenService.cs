namespace CareerGauge.Application.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(
        string userId,
        string email,
        int learnerId);
}