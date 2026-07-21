using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices;

/// <summary>
/// Worker nền rà hạng thành viên định kỳ. Gọi <see cref="ILoyaltyService.ReevaluateAllTiersAsync"/>
/// để HẠ hạng những khách không đủ số booking trong ~30 ngày gần nhất — kể cả khách KHÔNG có
/// booking nào (trường hợp mà luồng checkout không bao giờ chạm tới được).
///
/// Sweep đầu tiên chạy NGAY khi khởi động, sau đó lặp lại mỗi <see cref="SweepInterval"/>.
/// </summary>
public class TierMaintenanceService : BackgroundService
{
    private static readonly TimeSpan SweepInterval = TimeSpan.FromHours(6);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TierMaintenanceService> _logger;

    public TierMaintenanceService(IServiceScopeFactory scopeFactory, ILogger<TierMaintenanceService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var loyalty = scope.ServiceProvider.GetRequiredService<ILoyaltyService>();
                await loyalty.ReevaluateAllTiersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // Không để lỗi tạm thời giết worker; log rồi thử lại lần sweep sau.
                _logger.LogError(ex, "Tier maintenance sweep failed.");
            }

            try
            {
                await Task.Delay(SweepInterval, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }
}
