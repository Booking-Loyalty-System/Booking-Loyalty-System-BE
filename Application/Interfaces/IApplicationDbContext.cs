using System.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<WashPackage> WashPackages { get; }
    DbSet<WashBay> WashBays { get; }
    DbSet<TimeSlot> TimeSlots { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<LoyaltyTransaction> LoyaltyTransactions { get; }
    DbSet<Tier> Tiers { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Reward> Rewards { get; }
    DbSet<RewardRedemption> RewardRedemptions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction. Used to make read-validate-write sequences
    /// (e.g. reserving a wash-bay time slot) atomic and free of race conditions.
    /// Defaults to Serializable so concurrent reservations cannot both observe a
    /// slot as free and then both book it.
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default);
}
