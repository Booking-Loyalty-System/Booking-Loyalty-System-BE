using Application.Interfaces;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

/// <summary>
/// Background worker that releases slots held by bookings that were never confirmed.
/// In this shop's flow a booking has NO online payment: the customer books, staff later
/// calls and flips it Pending -> Confirmed. So a Pending booking is a legitimate hold that
/// must survive until its appointment time — it is only cancelled once its start time has
/// passed and staff still never confirmed it, freeing the slot back to the pool.
/// (Confirmed/CheckedIn no-shows are handled separately by AutoNoShowService.)
/// </summary>
public class PendingBookingCleanupService : BackgroundService
{
    private static readonly TimeSpan SweepInterval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingBookingCleanupService> _logger;
    private readonly TimeZoneInfo _shopTimeZone;

    public PendingBookingCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<PendingBookingCleanupService> logger,
        TimeZoneInfo shopTimeZone)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _shopTimeZone = shopTimeZone;
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

        var nowUtc = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(nowUtc, _shopTimeZone));

        // Candidates: still Pending and on/before today. The exact "start time passed" check
        // needs a timezone conversion of BookingDate + StartTime, done in memory below.
        var candidates = await context.Bookings
            .Where(b => b.Status == BookingStatus.Pending && b.BookingDate <= today)
            .ToListAsync(ct);

        if (candidates.Count == 0)
            return;

        var expiredCount = 0;
        foreach (var booking in candidates)
        {
            // Interpret BookingDate + StartTime in the shop's local timezone, compare in UTC.
            var localStart = booking.BookingDate.ToDateTime(booking.StartTime); // DateTimeKind.Unspecified
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, _shopTimeZone);

            if (nowUtc < startUtc)
                continue; // Appointment time not reached yet — keep holding the slot.

            booking.Status = BookingStatus.Cancelled;
            booking.CancellationReason = "Booking was not confirmed by staff before the appointment time.";
            booking.UpdatedAt = nowUtc;
            // Capacity-based slots: a Cancelled booking no longer counts toward
            // BranchTimeSlot.MaxCapacity, so the slot frees up automatically.
            expiredCount++;
        }

        if (expiredCount == 0)
            return;

        await context.SaveChangesAsync(ct);
        _logger.LogInformation("Cancelled {Count} unconfirmed booking(s) past their appointment time.", expiredCount);
    }
}
