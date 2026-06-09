using Application.Common;
using Application.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        
        string credentialPath = Path.Combine(environment.ContentRootPath, "firebase-admin-key.json");
        
        // Kiểm tra xem file có tồn tại không để tránh crash app khi quên bỏ file key ở deploy
        if (File.Exists(credentialPath))
        {
            // Tránh lỗi khởi tạo trùng lặp nếu có nhiều chỗ gọi
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(credentialPath),
                });
            }
        }
        else
        {
            // Ông có thể throw exception hoặc ghi log ở đây tùy ý
            Console.WriteLine("⚠️ WARNING: Không tìm thấy file firebase-admin-key.json tại tầng API!");
        }

        services.Configure<BookingOptions>(configuration.GetSection("Booking"));
        services.Configure<LoyaltyOptions>(configuration.GetSection("Loyalty"));

        services.AddMemoryCache();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoyaltyService, LoyaltyService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IWashPackageService, WashPackageService>();
        services.AddScoped<IWashBayService, WashBayService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IOtpService, FirebaseService>();

        return services;
    }
}
