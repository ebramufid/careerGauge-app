using CareerGauge.Application.Authentication.DTOs;

namespace CareerGauge.Application.Authentication;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request)
    {
        var isValid = await _identityService.ValidateCredentialsAsync(
            request.Email,
            request.Password);

        if (!isValid)
        {
            return null;
        }

        var learnerId = await _identityService.GetLearnerIdAsync(
            request.Email);

        if (learnerId is null)
        {
            return null;
        }

        var userId = await _identityService.GetUserIdAsync(
            request.Email);

        if (userId is null)
        {
            return null;
        }

        var accessToken = _jwtTokenService.GenerateToken(
            userId,
            request.Email,
            learnerId.Value);

        return new LoginResponse
        {
            AccessToken = accessToken,
            LearnerId = learnerId.Value,
            Email = request.Email
        };
    }
}