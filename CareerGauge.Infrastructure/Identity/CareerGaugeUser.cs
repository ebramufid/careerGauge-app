using Microsoft.AspNetCore.Identity;

namespace CareerGauge.Infrastructure.Identity;

public class CareerGaugeUser : IdentityUser
{
    public int LearnerId { get; set; }
}