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

    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<WashBay> WashBays => Set<WashBay>();
    public DbSet<WashPackage> WashPackages => Set<WashPackage>();

    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>(); 

    public DbSet<WashBayTimeSlot> WashBayTimeSlots => Set<WashBayTimeSlot>(); 

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Transaction> Transactions => Set<Transaction>(); 
    public DbSet<LoyaltyTransaction> LoyaltyTransactions => Set<LoyaltyTransaction>();
    public DbSet<Reward> Rewards => Set<Reward>(); 
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<CustomerPromotion> CustomerPromotions => Set<CustomerPromotion>();
    public DbSet<PromotionBranch> PromotionBranches => Set<PromotionBranch>();
    public DbSet<TierPromotion> TierPromotions => Set<TierPromotion>(); // Bổ sung bảng TierPromotion trung gian
    public DbSet<Notification> Notifications => Set<Notification>();

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