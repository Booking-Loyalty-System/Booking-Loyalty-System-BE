using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BackgroundServices;

public class NotificationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(IServiceProvider serviceProvider, ILogger<NotificationWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Worker bắt đầu chạy.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var expiringRewards = await dbContext.CustomerPromotions
                        .Include(ur => ur.Promotion)
                        .Include(ur => ur.Customer)
                        .Where(ur => !ur.IsUsed 
                                  && ur.ExpiryDate <= DateTime.UtcNow.AddDays(3) 
                                  && ur.ExpiryDate > DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    foreach (var ur in expiringRewards)
                    {
                        // 2. Kiểm tra xem đã gửi thông báo cho cái quà cụ thể này chưa
                        var exists = await dbContext.Notifications.AnyAsync(n => 
                            n.ReferenceId == ur.Id && n.Type == "RewardExpiry", stoppingToken);
                        
                        if (!exists)
                        {
                            var notification = new Notification
                            {
                                Title = "Quà tặng sắp hết hạn!",
                                Type = "RewardExpiry",
                                ReferenceId = ur.Id,
                                UserId = ur.Customer.UserId   
                            };
                            dbContext.Notifications.Add(notification);
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chạy Worker quét thông báo.");
            }

            // Đợi 1 tiếng quét lại một lần
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}