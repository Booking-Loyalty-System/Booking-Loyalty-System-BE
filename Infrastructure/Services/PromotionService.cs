using Application.DTOs.Promotion;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PromotionService : IPromotionService
{
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

        var promotions = await _context.Promotions
            .Include(p => p.TierPromotions)
            .Include(p => p.PromotionBranches)
            .Where(p => p.IsActive
                && p.StartDate <= now
                && p.EndDate >= now
                && (p.MaxUses == null || p.UsedCount < p.MaxUses))
            .OrderByDescending(p => p.PriorityLevel)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();

        // Khách đã đăng nhập: chỉ trả về các KM mà khách dùng được ngay (đúng hạng + đang trong tuần sinh nhật).
        // Điều kiện chi nhánh/địa chỉ kiểm tra lúc Preview/Apply vì list chưa biết khách chọn chi nhánh nào.
        if (userId is not null)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId.Value);

            if (customer is not null)
            {
                promotions = promotions
                    .Where(p => IsTierEligible(p, customer) && IsBirthdayEligible(p, customer, now))
                    .ToList();
            }
        }

        return promotions.Select(MapToResponse).ToList();
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
            CreatedAt = DateTime.UtcNow
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

        // Reserve one use. Caller's SaveChanges/transaction commits this together with the booking.
        promotion.UsedCount += 1;

        return (promotion.Id, discount);
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

        if (promotion.MaxUses.HasValue && promotion.UsedCount >= promotion.MaxUses.Value)
            throw new AppException("This promotion has reached its usage limit.", 400);

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
