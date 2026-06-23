using Application.Common;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

/// <summary>
/// Background worker that cancels bookings left unpaid past the VNPay payment window,
/// releasing their reserved bay/time slot back to the pool. A booking stays Pending
/// from creation until a successful VNPay IPN flips it to Confirmed; anything still
/// Pending after PaymentTimeoutMinutes is treated as abandoned.
/// </summary>
public class PendingBookingCleanupService : BackgroundService
{
    private static readonly TimeSpan SweepInterval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingBookingCleanupService> _logger;
    private readonly int _timeoutMinutes;

    public PendingBookingCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<PendingBookingCleanupService> logger,
        IOptions<VnPayOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _timeoutMinutes = options.Value.PaymentTimeoutMinutes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SweepExpiredAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // Never let a transient failure kill the worker; log and retry next sweep.
                _logger.LogError(ex, "Pending-booking cleanup sweep failed.");
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

    private async Task SweepExpiredAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var cutoff = DateTime.UtcNow.AddMinutes(-_timeoutMinutes);

        var expired = await context.Bookings
            .Where(b => b.Status == BookingStatus.Pending && b.CreatedAt < cutoff)
            .ToListAsync(ct);

        if (expired.Count == 0)
            return;

        foreach (var booking in expired)
        {
            booking.Status = BookingStatus.Cancelled;
            booking.CancellationReason = "Payment was not completed within the allowed time.";
            booking.UpdatedAt = DateTime.UtcNow;
            // Capacity-based slots: a Cancelled booking no longer counts toward
            // BranchTimeSlot.MaxCapacity, so the slot frees up automatically.
        }

        await context.SaveChangesAsync(ct);
        _logger.LogInformation("Cancelled {Count} unpaid booking(s) past the payment window.", expired.Count);
    }
}
