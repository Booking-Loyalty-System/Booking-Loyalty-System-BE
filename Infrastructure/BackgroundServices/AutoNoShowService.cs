using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.BackgroundServices;

/// <summary>
/// Background worker that auto-marks no-shows. A booking that reached Confirmed or CheckedIn
/// (the customer was expected) but never started service is treated as a no-show once its
/// start time has passed by more than <see cref="BookingOptions.NoShowGraceMinutes"/>.
///
/// Scope of statuses mirrors the manual StaffBookingService.NoShowAsync (Confirmed, CheckedIn):
///   - Pending bookings are handled by PendingBookingCleanupService (unpaid -> Cancelled).
///   - Queued / InProgress mean the customer already arrived, so they are never no-showed here.
/// Confirmed/CheckedIn bookings hold no wash bay (a bay is only assigned at Queue), so no bay
/// release is needed.
/// </summary>
public class AutoNoShowService : BackgroundService
{
    private static readonly TimeSpan SweepInterval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AutoNoShowService> _logger;
    private readonly TimeZoneInfo _shopTimeZone;
    private readonly int _graceMinutes;

    public AutoNoShowService(
        IServiceScopeFactory scopeFactory,
        ILogger<AutoNoShowService> logger,
        TimeZoneInfo shopTimeZone,
        IOptions<BookingOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _shopTimeZone = shopTimeZone;
        _graceMinutes = options.Value.NoShowGraceMinutes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SweepNoShowsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // Never let a transient failure kill the worker; log and retry next sweep.
                _logger.LogError(ex, "Auto no-show sweep failed.");
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

    private async Task SweepNoShowsAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var loyalty = scope.ServiceProvider.GetRequiredService<ILoyaltyService>();

        var nowUtc = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(nowUtc, _shopTimeZone));

        // Candidates: still waiting for the customer, appointment on/before today.
        // The exact "past start time + grace" check is done in memory because it needs a
        // timezone conversion of BookingDate + StartTime that the provider can't translate to SQL.
        var candidates = await context.Bookings
            .Include(b => b.Customer)
            .Where(b => (b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
                     && b.BookingDate <= today)
            .ToListAsync(ct);

        if (candidates.Count == 0)
            return;

        var toNotify = new List<Booking>();

        foreach (var booking in candidates)
        {
            // Interpret BookingDate + StartTime in the shop's local timezone, then compare in UTC.
            var localStart = booking.BookingDate.ToDateTime(booking.StartTime); // DateTimeKind.Unspecified
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, _shopTimeZone);

            if (nowUtc < startUtc.AddMinutes(_graceMinutes))
                continue; // Grace period not elapsed yet.

            booking.Status = BookingStatus.NoShow;
            booking.CancellationReason = $"Auto no-show: customer did not arrive within {_graceMinutes} minutes of the booked time.";
            booking.UpdatedAt = nowUtc;
            // NOTE: A voucher/promotion applied to this booking is NOT auto-restored here, and a
            // paid booking is NOT auto-refunded — those are business-policy decisions. Wire them in
            // if the team decides no-shows should return the voucher / trigger a refund.
            toNotify.Add(booking);
        }

        if (toNotify.Count == 0)
            return;

        await context.SaveChangesAsync(ct);

        // Deduct the no-show penalty (spendable points, clamped at 0) and tell each customer.
        foreach (var booking in toNotify)
        {
            await loyalty.ApplyNoShowPenaltyAsync(booking.Id, ct);

            if (booking.Customer is null)
                continue;

            await notifications.CreateNotificationAsync(
                booking.Customer.UserId,
                "Lịch rửa xe bị huỷ do không đến",
                $"Lịch đặt {booking.BookingCode} đã bị đánh dấu \"Không đến\" vì bạn không có mặt trong vòng {_graceMinutes} phút sau giờ hẹn.",
                "Booking");
        }

        _logger.LogInformation("Auto no-show: marked {Count} booking(s) as NoShow.", toNotify.Count);
    }
}
