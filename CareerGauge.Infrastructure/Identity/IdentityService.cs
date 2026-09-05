using CareerGauge.Application.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CareerGauge.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<CareerGaugeUser> _userManager;

    public IdentityService(
        UserManager<CareerGaugeUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ValidateCredentialsAsync(
        string email,
        string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(
            user,
            password);
    }

    public async Task<int?> GetLearnerIdAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user?.LearnerId;
    }

    public async Task<string?> GetUserIdAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user?.Id;
    }
}