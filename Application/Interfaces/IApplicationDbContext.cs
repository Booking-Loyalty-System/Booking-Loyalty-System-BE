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
    DbSet<Tier> Tiers { get; }

    // --- Hệ thống Cửa hàng & Vận hành ---
    DbSet<Branch> Branches { get; }
    DbSet<WashBay> WashBays { get; }
    DbSet<WashPackage> WashPackages { get; }
    DbSet<TimeSlot> TimeSlots { get; } 
    
    // ĐÂY RỒI: Khai báo bảng trung gian vào đây để BookingService bốc lịch gối đầu
    DbSet<WashBayTimeSlot> WashBayTimeSlots { get; } 

    // --- Hệ thống Đặt lịch & Giao dịch ---
    DbSet<Booking> Bookings { get; }
    DbSet<Transaction> Transactions { get; } 
    DbSet<LoyaltyTransaction> LoyaltyTransactions { get; }
    DbSet<Reward> Rewards { get; } 

    // --- Hệ thống Ưu đãi & Khuyến mãi (Promotions) ---
    DbSet<Promotion> Promotions { get; }
    DbSet<CustomerPromotion> CustomerPromotions { get; }
    DbSet<PromotionBranch> PromotionBranches { get; }
    DbSet<TierPromotion> TierPromotions { get; } 

    // --- Tiện ích ---
    DbSet<Notification> Notifications { get; }

    // --- Các hàm Core EF Core ---
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