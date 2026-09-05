namespace CareerGauge.Application.Authentication;

public interface IIdentityService
{
    Task<bool> ValidateCredentialsAsync(
        string email,
        string password);

    Task<int?> GetLearnerIdAsync(
        string email);

    Task<string?> GetUserIdAsync(
        string email);
}