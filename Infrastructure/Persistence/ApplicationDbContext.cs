using System.Data;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Tier> Tiers => Set<Tier>();
    public DbSet<AddOn> AddOns => Set<AddOn>();
    public DbSet<BookingAddOn> BookingAddOns => Set<BookingAddOn>();
    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<WashBay> WashBays => Set<WashBay>();
    public DbSet<WashPackage> WashPackages => Set<WashPackage>();

    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>(); 

    public DbSet<BranchTimeSlot> BranchTimeSlots => Set<BranchTimeSlot>(); 

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Transaction> Transactions => Set<Transaction>(); 
    public DbSet<Point> Points => Set<Point>();
    public DbSet<PointHistory> PointHistories => Set<PointHistory>();
    public DbSet<Reward> Rewards => Set<Reward>(); 
    
    // 🔥 BỔ SUNG DÒNG NÀY ĐỂ FIX LỖI INTERFACE
    public DbSet<RewardRedemption> RewardRedemptions => Set<RewardRedemption>(); 

    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<CustomerPromotion> CustomerPromotions => Set<CustomerPromotion>();
    public DbSet<PromotionBranch> PromotionBranches => Set<PromotionBranch>();
    public DbSet<TierPromotion> TierPromotions => Set<TierPromotion>(); 
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Staff> Staffs => Set<Staff>();
    
    public Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.Serializable,
        CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(isolationLevel, cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}