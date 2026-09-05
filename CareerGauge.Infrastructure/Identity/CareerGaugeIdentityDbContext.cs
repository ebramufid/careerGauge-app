using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerGauge.Infrastructure.Identity;

public class CareerGaugeIdentityDbContext
    : IdentityDbContext<CareerGaugeUser>
{
    public CareerGaugeIdentityDbContext(
        DbContextOptions<CareerGaugeIdentityDbContext> options)
        : base(options)
    {
    }
}