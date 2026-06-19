using Application.DTOs.Tier;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TierService : ITierService
{
    private readonly IApplicationDbContext _context;

    public TierService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TierResponse>> GetAllTiersAsync()
    {
        var tiers = await _context.Tiers
            .OrderBy(t => t.Level) // Sắp xếp theo cấp độ ưu tiên
    public async Task<List<TierResponse>> GetAllAsync()
    {
        var tiers = await _context.Tiers
            .OrderBy(t => t.MinPointsRequired)
            .ToListAsync();

        return tiers.Select(MapToResponse).ToList();
    }

    public async Task<TierResponse> GetTierByIdAsync(Guid id)
    {
        var tier = await _context.Tiers
            .FirstOrDefaultAsync(t => t.Id == id)
    public async Task<CustomerTierResponse> GetMyTierAsync(Guid userId)
    {
        var customer = await _context.Customers
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        // Find next tier
        var nextTier = await _context.Tiers
            .Where(t => t.MinPointsRequired > customer.LifetimePoints)
            .OrderBy(t => t.MinPointsRequired)
            .FirstOrDefaultAsync();

        // Calculate maintenance: sum of Earn points in the last 90 days
        var windowStart = DateTime.UtcNow.AddDays(-90);
        var recentPoints = await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id
                         && lt.Type == LoyaltyTransactionType.Earn
                         && lt.CreatedAt >= windowStart)
            .SumAsync(lt => lt.Points);

        // Days remaining: how many days until the oldest transaction in the 90-day window falls off
        var oldestInWindow = await _context.LoyaltyTransactions
            .Where(lt => lt.CustomerId == customer.Id
                         && lt.Type == LoyaltyTransactionType.Earn
                         && lt.CreatedAt >= windowStart)
            .OrderBy(lt => lt.CreatedAt)
            .Select(lt => (DateTime?)lt.CreatedAt)
            .FirstOrDefaultAsync();

        var daysRemaining = oldestInWindow.HasValue
            ? Math.Max(0, (int)(90 - (DateTime.UtcNow - oldestInWindow.Value).TotalDays))
            : 90;

        var maintenanceRequired = customer.Tier.MaintenancePoints;

        return new CustomerTierResponse
        {
            CurrentTier = MapToResponse(customer.Tier),
            LifetimePoints = customer.LifetimePoints,
            NextTier = nextTier != null
                ? new NextTierInfo
                {
                    TierName = nextTier.TierName,
                    MinPointsRequired = nextTier.MinPointsRequired,
                    PointsNeeded = nextTier.MinPointsRequired - customer.LifetimePoints,
                    ProgressPercent = Math.Round(
                        (double)customer.LifetimePoints / nextTier.MinPointsRequired * 100, 1)
                }
                : null,
            Maintenance = new MaintenanceInfo
            {
                RequiredPoints = maintenanceRequired,
                RecentPoints = recentPoints,
                DaysRemaining = daysRemaining,
                IsSafe = recentPoints >= maintenanceRequired
            }
        };
    }

    public async Task<TierResponse> GetByIdAsync(Guid id)
    {
        var tier = await _context.Tiers.FindAsync(id)
            ?? throw new AppException("Tier not found.", 404);

        return MapToResponse(tier);
    }

    public async Task<TierResponse> CreateTierAsync(CreateTierRequest request)
    {
        // Kiểm tra xem tên Tier đã tồn tại chưa
        var existingName = await _context.Tiers
            .AnyAsync(t => t.TierName == request.TierName);

        if (existingName)
            throw new AppException("A tier with this name already exists.", 409);

    public async Task<TierResponse> CreateAsync(CreateTierRequest request)
    {
        var tier = new Tier
        {
            Id = Guid.NewGuid(),
            TierName = request.TierName,
            PointRate = request.PointRate,
            BookingWindow = request.BookingWindow,
            Level = Enum.Parse<PriorityLevel>(request.Level) // Parse giống VehicleType
            Level = Enum.Parse<PriorityLevel>(request.Level, true),
            MinPointsRequired = request.MinPointsRequired,
            MaintenancePoints = request.MaintenancePoints
        };

        _context.Tiers.Add(tier);
        await _context.SaveChangesAsync();

        return MapToResponse(tier);
    }

    public async Task<TierResponse> UpdateTierAsync(Guid id, UpdateTierRequest request)
    {
        var tier = await _context.Tiers
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new AppException("Tier not found.", 404);

        // Nếu thay đổi tên, cần kiểm tra trùng lặp với tên khác trong DB
        if (tier.TierName != request.TierName)
        {
            var nameExists = await _context.Tiers.AnyAsync(t => t.TierName == request.TierName);
            if (nameExists) 
                throw new AppException("A tier with this name already exists.", 409);
        }

        tier.TierName = request.TierName;
        tier.PointRate = request.PointRate;
        tier.BookingWindow = request.BookingWindow;
        tier.Level = Enum.Parse<PriorityLevel>(request.Level);
    public async Task<TierResponse> UpdateAsync(Guid id, UpdateTierRequest request)
    {
        var tier = await _context.Tiers.FindAsync(id)
            ?? throw new AppException("Tier not found.", 404);

        if (request.TierName != null) tier.TierName = request.TierName;
        if (request.PointRate != null) tier.PointRate = request.PointRate.Value;
        if (request.BookingWindow != null) tier.BookingWindow = request.BookingWindow.Value;
        if (request.Level != null) tier.Level = Enum.Parse<PriorityLevel>(request.Level, true);
        if (request.MinPointsRequired != null) tier.MinPointsRequired = request.MinPointsRequired.Value;
        if (request.MaintenancePoints != null) tier.MaintenancePoints = request.MaintenancePoints.Value;

        await _context.SaveChangesAsync();

        return MapToResponse(tier);
    }

    public async Task DeleteTierAsync(Guid id)
    {
        var tier = await _context.Tiers
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new AppException("Tier not found.", 404);

        // Validate Ràng buộc (Quan trọng): Không cho phép xóa nếu đang có khách hàng ở Tier này
        var isUsedByCustomers = await _context.Customers.AnyAsync(c => c.TierId == id);
        
        if (isUsedByCustomers)
            throw new AppException("Cannot delete this tier because there are customers associated with it.", 400);
    public async Task DeleteAsync(Guid id)
    {
        var tier = await _context.Tiers
            .Include(t => t.Customers)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new AppException("Tier not found.", 404);

        if (tier.Customers.Any())
            throw new AppException("Cannot delete tier that still has customers.", 400);

        _context.Tiers.Remove(tier);
        await _context.SaveChangesAsync();
    }

    private static TierResponse MapToResponse(Tier tier)
    {
        return new TierResponse
        {
            Id = tier.Id,
            TierName = tier.TierName,
            PointRate = tier.PointRate,
            BookingWindow = tier.BookingWindow,
            Level = tier.Level.ToString()
        };
    }
}
    private static TierResponse MapToResponse(Tier tier) => new()
    {
        Id = tier.Id,
        TierName = tier.TierName,
        Level = tier.Level.ToString(),
        PointRate = tier.PointRate,
        BookingWindow = tier.BookingWindow,
        MinPointsRequired = tier.MinPointsRequired,
        MaintenancePoints = tier.MaintenancePoints,
        Benefits = GenerateBenefits(tier)
    };

    private static List<string> GenerateBenefits(Tier tier) =>
    [
        $"Tích {tier.PointRate:0.#}x điểm mỗi 1.000đ",
        $"Đặt trước {tier.BookingWindow} ngày",
        tier.MaintenancePoints > 0
            ? $"Cần {tier.MaintenancePoints} điểm/90 ngày để duy trì"
            : "Không yêu cầu duy trì"
    ];
}
