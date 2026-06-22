using System.Text.Json;
using Application.Common;
using Application.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.BackgroundServices;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        
        string credentialPath = Path.Combine(environment.ContentRootPath, "firebase-admin-key.json");
        
        // Kiểm tra xem file có tồn tại không để tránh crash app khi quên bỏ file key ở deploy
        var projectId = configuration["Firebase:ProjectId"];
        var privateKeyId = configuration["Firebase:PrivateKeyId"];
        var privateKey = configuration["Firebase:PrivateKey"];
        var clientEmail = configuration["Firebase:ClientEmail"];

// 2. Kiểm tra xem đã điền đủ cấu hình ở appsettings chưa
        if (!string.IsNullOrEmpty(projectId) && !string.IsNullOrEmpty(privateKey))
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                // Xử lý ký tự xuống dòng \n trong private key để tránh lỗi định dạng của Google
                var formattedPrivateKey = privateKey.Replace("\\n", "\n");

                // Tạo đúng cấu hình Object JSON theo định dạng chuẩn của Google
                var googleConfigObject = new
                {
                    type = "service_account",
                    project_id = projectId,
                    private_key_id = privateKeyId,
                    private_key = formattedPrivateKey,
                    client_email = clientEmail
                };

                // Chuyển Object thành chuỗi String JSON
                string googleJsonString = JsonSerializer.Serialize(googleConfigObject);

                // Nạp cấu hình từ chuỗi String thay vì đọc file vật lý!
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(googleJsonString),
                });
            }
        }
        else
        {
            Console.WriteLine("⚠️ WARNING: Chưa cấu hình thông tin Firebase trong appsettings.json!");
        }

        services.Configure<BookingOptions>(configuration.GetSection("Booking"));
        services.Configure<LoyaltyOptions>(configuration.GetSection("Loyalty"));
        services.AddScoped(provider => 
            provider.GetRequiredService<IOptions<BookingOptions>>().Value);
        services.AddSingleton<TimeZoneInfo>(provider => 
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        
        services.AddMemoryCache();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILoyaltyService, LoyaltyService>();
        services.AddScoped<IRewardService, RewardService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IStaffBookingService, StaffBookingService>();
        services.AddScoped<IWashPackageService, WashPackageService>();
        services.AddScoped<IWashBayService, WashBayService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<ITierService, TierService>();
        services.AddScoped<ITimeSlotService, TimeSlotService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddHostedService<NotificationWorker>();

        return services;
    }
}
