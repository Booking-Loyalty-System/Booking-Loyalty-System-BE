using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Persistence;
using Domain.Entities;
using Domain.Enums;
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

                    var expiringRewards = await dbContext.RewardRedemptions
                        .Include(rr => rr.Customer)
                        .Where(rr => rr.Status == RedemptionStatus.Pending
                                  && rr.ExpiryDate != null
                                  && rr.ExpiryDate <= DateTime.UtcNow.AddDays(3)
                                  && rr.ExpiryDate > DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    foreach (var rr in expiringRewards)
                    {
                        // 2. Kiểm tra xem đã gửi thông báo cho voucher cụ thể này chưa
                        var exists = await dbContext.Notifications.AnyAsync(n =>
                            n.ReferenceId == rr.Id && n.Type == "RewardExpiry", stoppingToken);

                        if (!exists)
                        {
                            var notification = new Notification
                            {
                                Title = "Quà tặng sắp hết hạn!",
                                Type = "RewardExpiry",
                                ReferenceId = rr.Id,
                                UserId = rr.Customer.UserId
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