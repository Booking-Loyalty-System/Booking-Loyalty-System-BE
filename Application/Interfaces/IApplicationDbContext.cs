using System.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    // --- Hệ thống Tài khoản & Khách hàng ---
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<WashBay> WashBays { get; }
    DbSet<WashPackage> WashPackages { get; }
    DbSet<TimeSlot> TimeSlots { get; } 
    DbSet<Staff> Staffs { get; }
    // ĐÂY RỒI: Khai báo bảng trung gian vào đây để BookingService bốc lịch gối đầu
    DbSet<BranchTimeSlot> BranchTimeSlots { get; } 

    // --- Hệ thống Đặt lịch & Giao dịch ---
    DbSet<Booking> Bookings { get; }
    DbSet<Transaction> Transactions { get; } 
    DbSet<LoyaltyTransaction> LoyaltyTransactions { get; }
    DbSet<CustomerPromotion> CustomerPromotions { get; }
    DbSet<PromotionBranch> PromotionBranches { get; }
    DbSet<TierPromotion> TierPromotions { get; } 

    // --- Tiện ích ---
    DbSet<Notification> Notifications { get; }

    // --- Các hàm Core EF Core ---
    DbSet<Tier> Tiers { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Reward> Rewards { get; }
    DbSet<RewardRedemption> RewardRedemptions { get; }
    DbSet<Promotion> Promotions { get; }
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