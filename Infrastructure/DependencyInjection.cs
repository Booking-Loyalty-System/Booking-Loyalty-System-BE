using Application.Common;
using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.Configure<BookingOptions>(configuration.GetSection("Booking"));
        services.Configure<LoyaltyOptions>(configuration.GetSection("Loyalty"));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoyaltyService, LoyaltyService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IWashPackageService, WashPackageService>();
        services.AddScoped<IWashBayService, WashBayService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAdminUserService, AdminUserService>();

        return services;
    }
}
