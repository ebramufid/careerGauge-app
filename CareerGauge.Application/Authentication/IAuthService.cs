using CareerGauge.Application.Authentication.DTOs;

namespace CareerGauge.Application.Authentication;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}