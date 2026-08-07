using Application.DTOs.Promotion;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PromotionService : IPromotionService
{
    /// <summary>Trần giảm giá: một khuyến mãi không được giảm quá 20% giá gói dịch vụ.</summary>
    private const decimal MaxDiscountRate = 0.20m;

    private readonly IApplicationDbContext _context;

    public PromotionService(IApplicationDbContext context)
    {
        _context = context;
    }

    // ----- Catalog management -----

    public async Task<List<PromotionResponse>> GetAllAsync()
    {
        var promotions = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return promotions.Select(MapToResponse).ToList();
    }

    public async Task<List<PromotionResponse>> GetActiveAsync(Guid? userId = null)
    {
        var now = DateTime.UtcNow;

        Customer? customer = null;
        if (userId.HasValue)
        {
            customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId.Value);
        }

        var promotions = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .Where(p => p.IsActive
                && p.StartDate <= now
                && p.EndDate >= now)
            .OrderByDescending(p => p.PriorityLevel)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();

        var filteredPromotions = promotions.Where(p =>
            IsTierEligible(p, customer) &&
            IsBirthdayEligible(p, customer, now)
        ).ToList();

        return filteredPromotions.Select(MapToResponse).ToList();
    }

    // ----- Eligibility (sinh nhật / hạng / chi nhánh) -----

    /// <summary>Hạng: nếu KM có gắn TierPromotion thì khách phải đúng hạng; không gắn = mọi hạng.</summary>
    private static bool IsTierEligible(Promotion promo, Customer customer)
        => promo.TierPromotions.Count == 0
           || promo.TierPromotions.Any(tp => tp.TierId == customer.TierId);

    /// <summary>Sinh nhật: nếu KM yêu cầu sinh nhật thì chỉ dùng trong TUẦN sinh nhật (sinh nhật ± 3 ngày).</summary>
    private static bool IsBirthdayEligible(Promotion promo, Customer customer, DateTime now)
    {
        if (!promo.RequiresBirthday) return true;
        if (!customer.DateOfBirth.HasValue) return false;

        var dob = customer.DateOfBirth.Value;
        var today = now.Date;
        // Xét sinh nhật ở năm trước/nay/sau để xử lý trường hợp vắt qua giao thừa
        // (vd sinh 31/12, hôm nay 02/01 vẫn nằm trong cửa sổ ±3 ngày).
        foreach (var year in new[] { today.Year - 1, today.Year, today.Year + 1 })
        {
            if (Math.Abs((today - BirthdayInYear(dob, year)).TotalDays) <= 3)
                return true;
        }
        return false;
    }

    /// <summary>Quy ngày sinh về một năm cụ thể; sinh 29/02 vào năm không nhuận lùi về 28/02.</summary>
    private static DateTime BirthdayInYear(DateTime dob, int year)
    {
        var day = (dob.Month == 2 && dob.Day == 29 && !DateTime.IsLeapYear(year)) ? 28 : dob.Day;
        return new DateTime(year, dob.Month, day);
    }

    /// <summary>Chi nhánh/địa chỉ: nếu KM có gắn PromotionBranch (active) thì khách phải đặt tại chi nhánh đó.</summary>
    private static bool IsBranchEligible(Promotion promo, Guid? branchId)
    {
        var restricted = promo.PromotionBranches.Where(pb => pb.IsActive).ToList();
        if (restricted.Count == 0) return true; // không giới hạn chi nhánh
        return branchId.HasValue && restricted.Any(pb => pb.BranchId == branchId.Value);
    }

    public async Task<PromotionResponse?> GetByIdAsync(Guid id)
    {
        var promotion = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .FirstOrDefaultAsync(p => p.Id == id);
        return promotion == null ? null : MapToResponse(promotion);
    }

    public async Task<PromotionResponse> CreateAsync(CreatePromotionRequest request)
    {
        var code = request.Code.Trim().ToUpperInvariant();

        var exists = await _context.Promotions.AnyAsync(p => p.Code == code);
        if (exists)
            throw new AppException("A promotion with this code already exists.", 409);

        var promotion = new Promotion
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = request.Name,
            Description = request.Description,
            DiscountType = Enum.Parse<DiscountType>(request.DiscountType, ignoreCase: true),
            DiscountValue = request.DiscountValue,
            PriorityLevel = request.PriorityLevel,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxUses = request.MaxUses,
            UsedCount = 0,
            MinSpend = request.MinSpend,
            IsActive = true,
            RequiresBirthday = request.RequiresBirthday,
            CreatedAt = DateTime.UtcNow,
            MaxDiscount = request.MaxDiscount
        };

        // Gắn điều kiện hạng (TierPromotion) và chi nhánh/địa chỉ (PromotionBranch) nếu có.
        if (request.TierIds is { Count: > 0 })
            foreach (var tierId in request.TierIds.Distinct())
                promotion.TierPromotions.Add(new TierPromotion
                {
                    TierPromotionId = Guid.NewGuid(),
                    PromotionId = promotion.Id,
                    TierId = tierId
                });

        if (request.BranchIds is { Count: > 0 })
            foreach (var branchId in request.BranchIds.Distinct())
                promotion.PromotionBranches.Add(new PromotionBranch
                {
                    Id = Guid.NewGuid(),
                    PromotionId = promotion.Id,
                    BranchId = branchId,
                    IsActive = true
                });

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return MapToResponse(promotion);
    }

    public async Task<PromotionResponse> UpdateAsync(Guid id, UpdatePromotionRequest request)
    {
        var promotion = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new AppException("Promotion not found.", 404);

        if (request.Name != null) promotion.Name = request.Name;
        if (request.Description != null) promotion.Description = request.Description;
        if (request.DiscountType != null) promotion.DiscountType = Enum.Parse<DiscountType>(request.DiscountType, ignoreCase: true);
        if (request.DiscountValue.HasValue) promotion.DiscountValue = request.DiscountValue.Value;
        if (request.PriorityLevel.HasValue) promotion.PriorityLevel = request.PriorityLevel.Value;
        if (request.StartDate.HasValue) promotion.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) promotion.EndDate = request.EndDate.Value;
        if (request.MaxUses.HasValue) promotion.MaxUses = request.MaxUses.Value;
        if (request.MinSpend.HasValue) promotion.MinSpend = request.MinSpend.Value;
        if (request.IsActive.HasValue) promotion.IsActive = request.IsActive.Value;
        if (request.RequiresBirthday.HasValue) promotion.RequiresBirthday = request.RequiresBirthday.Value;

        // TierIds/BranchIds != null = thay thế toàn bộ danh sách (rỗng = gỡ hết giới hạn).
        if (request.TierIds != null)
        {
            _context.TierPromotions.RemoveRange(promotion.TierPromotions);
            foreach (var tierId in request.TierIds.Distinct())
                promotion.TierPromotions.Add(new TierPromotion
                {
                    TierPromotionId = Guid.NewGuid(),
                    PromotionId = promotion.Id,
                    TierId = tierId
                });
        }

        if (request.BranchIds != null)
        {
            _context.PromotionBranches.RemoveRange(promotion.PromotionBranches);
            foreach (var branchId in request.BranchIds.Distinct())
                promotion.PromotionBranches.Add(new PromotionBranch
                {
                    Id = Guid.NewGuid(),
                    PromotionId = promotion.Id,
                    BranchId = branchId,
                    IsActive = true
                });
        }

        await _context.SaveChangesAsync();

        return MapToResponse(promotion);
    }

    public async Task DeleteAsync(Guid id)
    {
        var promotion = await _context.Promotions.FindAsync(id)
            ?? throw new AppException("Promotion not found.", 404);

        _context.Promotions.Remove(promotion);
        await _context.SaveChangesAsync();
    }

    // ----- Application -----

    public async Task<PromotionPreviewResponse> PreviewAsync(string code, decimal subtotal, Guid userId, Guid? branchId = null)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var promotion = await LoadValidPromotionAsync(code, subtotal, customer, branchId);
        var discount = ComputeDiscount(promotion, subtotal);
        EnforceDiscountCap(discount, subtotal);

        return new PromotionPreviewResponse
        {
            Code = promotion.Code,
            Subtotal = subtotal,
            DiscountAmount = discount,
            FinalAmount = subtotal - discount
        };
    }

    public async Task<(Guid PromotionId, decimal DiscountAmount)> ApplyAsync(string code, decimal subtotal, Customer customer, Guid branchId)
    {
        var promotion = await LoadValidPromotionAsync(code, subtotal, customer, branchId);
        var discount = ComputeDiscount(promotion, subtotal);

        // Chặn trước khi giữ chỗ lượt dùng: nếu vượt trần thì không áp và không tăng UsedCount.
        EnforceDiscountCap(discount, subtotal);

        // Reserve one use. Caller's SaveChanges/transaction commits this together with the booking.
        promotion.UsedCount += 1;

        return (promotion.Id, discount);
    }

    public async Task<IEnumerable<object>> GetEligiblePromotionsAsync(Guid userId, Guid? branchId)
    {
        var now = DateTime.UtcNow;

        // 1. Lấy thông tin khách hàng để check Tier và Birthday
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (customer == null) return Enumerable.Empty<object>();

        // FIX 1: Lấy .Value để Dictionary chuẩn key là Guid
        var userPromoUsage = await _context.Bookings
            .Where(b => b.CustomerId == customer.Id && b.PromotionId != null && b.Status != BookingStatus.Cancelled)
            .GroupBy(b => b.PromotionId)
            .Select(g => new { PromotionId = g.Key!.Value, Count = g.Count() })
            .ToDictionaryAsync(x => x.PromotionId, x => x.Count);

        var query = _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .AsNoTracking()
            .Where(p => p.IsActive
                        && p.StartDate <= now
                        && p.EndDate >= now);

        // FIX 2: Lấy KM dành cho mọi người (không cấu hình Tier) HOẶC KM đúng Tier của khách
        query = query.Where(p => !p.TierPromotions.Any() || p.TierPromotions.Any(tp => tp.TierId == customer.TierId));

        var rawPromotions = await query.ToListAsync();
        var eligiblePromotions = new List<Promotion>();

        // 4. Lọc chi tiết ở Memory (Birthday & Branch)
        foreach (var promo in rawPromotions)
        {
            if (promo.MaxUses.HasValue)
            {
                // Do key giờ đã là Guid chuẩn, nên TryGetValue sẽ chạy chính xác 100%
                userPromoUsage.TryGetValue(promo.Id, out int usedCount);
                if (usedCount >= promo.MaxUses.Value)
                    continue; // Hết số lần -> Loại ngay lập tức
            }

            // Kiểm tra tuần sinh nhật (± 3 ngày) nếu promo yêu cầu
            if (promo.RequiresBirthday)
            {
                if (customer.DateOfBirth == null) continue;

                var birthdayThisYear = new DateTime(now.Year, customer.DateOfBirth.Value.Month, customer.DateOfBirth.Value.Day);
                var startBirthdayWindow = birthdayThisYear.AddDays(-3);
                var endBirthdayWindow = birthdayThisYear.AddDays(3);

                if (now < startBirthdayWindow || now > endBirthdayWindow)
                    continue;
            }

            // Kiểm tra Chi nhánh
            if (branchId.HasValue && promo.PromotionBranches.Any())
            {
                if (!promo.PromotionBranches.Any(pb => pb.BranchId == branchId.Value))
                    continue;
            }

            eligiblePromotions.Add(promo);
        }

        return eligiblePromotions
            .OrderByDescending(p => p.PriorityLevel)
            .Select(p => new
            {
                p.Id,
                p.Code,
                p.Name,
                p.Description,
                p.DiscountType,
                p.DiscountValue,
                p.MinSpend,
                p.PriorityLevel,
                p.MaxDiscount,
            });
    }

    /// <summary>Loads a promotion by code and enforces all eligibility rules, or throws.</summary>
    private async Task<Promotion> LoadValidPromotionAsync(string code, decimal subtotal, Customer customer, Guid? branchId)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new AppException("Promotion code is required.", 400);

        var normalized = code.Trim().ToUpperInvariant();
        var promotion = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .FirstOrDefaultAsync(p => p.Code == normalized)
            ?? throw new AppException("Promotion code not found.", 404);

        if (!promotion.IsActive)
            throw new AppException("This promotion is no longer active.", 400);

        var now = DateTime.UtcNow;
        if (now < promotion.StartDate || now > promotion.EndDate)
            throw new AppException("This promotion is not valid at this time.", 400);

        if (promotion.MaxUses.HasValue)
        {
            var userUsageCount = await _context.Bookings
                .CountAsync(b => b.CustomerId == customer.Id
                              && b.PromotionId == promotion.Id
                              && b.Status != BookingStatus.Cancelled);

            if (userUsageCount >= promotion.MaxUses.Value)
                throw new AppException($"Mã khuyến mãi này giới hạn {promotion.MaxUses.Value} lần/người. Bạn đã sử dụng hết, vui lòng chọn ưu đãi khác.", 400);
        }

        if (promotion.MinSpend.HasValue && subtotal < promotion.MinSpend.Value)
            throw new AppException($"This promotion requires a minimum spend of {promotion.MinSpend.Value:0.##}.", 400);

        // --- Điều kiện riêng của khách: đúng hạng / đúng tháng sinh nhật / đúng chi nhánh (địa chỉ) ---
        if (!IsTierEligible(promotion, customer))
            throw new AppException("Mã khuyến mãi này chỉ dành cho hạng thành viên khác.", 400);

        if (!IsBirthdayEligible(promotion, customer, now))
            throw new AppException("Mã khuyến mãi này chỉ áp dụng trong tuần sinh nhật của bạn (sinh nhật ± 3 ngày).", 400);

        if (!IsBranchEligible(promotion, branchId))
            throw new AppException("Mã khuyến mãi này không áp dụng cho chi nhánh đã chọn.", 400);

        return promotion;
    }

    /// <summary>Computes the discount, clamped so the final price never goes below zero.</summary>
    private static decimal ComputeDiscount(Promotion promotion, decimal subtotal)
    {
        var discount = promotion.DiscountType switch
        {
            DiscountType.Percentage => Math.Round(subtotal * promotion.DiscountValue / 100m, 2),
            DiscountType.FixedAmount => promotion.DiscountValue,
            _ => 0m
        };

        return Math.Min(discount, subtotal);
    }

    /// <summary>
    /// Chặn khi mức giảm vượt quá trần cho phép (<see cref="MaxDiscountRate"/> = 20% giá gói dịch vụ).
    /// Dùng chung cho cả Preview (để FE làm mờ ưu đãi không chọn được) và Apply (chặn tạo booking).
    /// Không thay đổi trạng thái khuyến mãi — chỉ từ chối áp cho gói giá thấp.
    /// </summary>
    private static void EnforceDiscountCap(decimal discount, decimal packagePrice)
    {
        if (packagePrice > 0 && discount > packagePrice * MaxDiscountRate)
            throw new AppException(
                $"Mã khuyến mãi này giảm quá {MaxDiscountRate * 100:0}% giá gói dịch vụ nên không thể áp dụng cho gói này. " +
                "Vui lòng chọn gói có giá cao hơn hoặc ưu đãi khác.", 400);
    }

    private static PromotionResponse MapToResponse(Promotion p) => new()
    {
        Id = p.Id,
        Code = p.Code,
        Name = p.Name,
        Description = p.Description,
        DiscountType = p.DiscountType.ToString(),
        DiscountValue = p.DiscountValue,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        MaxUses = p.MaxUses,
        UsedCount = p.UsedCount,
        MinSpend = p.MinSpend,
        IsActive = p.IsActive,
        RequiresBirthday = p.RequiresBirthday,
        // Rỗng = áp dụng mọi hạng / mọi chi nhánh; có phần tử = chỉ giới hạn trong danh sách đó.
        EligibleTierIds = p.TierPromotions.Select(tp => tp.TierId).ToList(),
        EligibleBranchIds = p.PromotionBranches.Where(pb => pb.IsActive).Select(pb => pb.BranchId).ToList(),
        CreatedAt = p.CreatedAt
    };
}
