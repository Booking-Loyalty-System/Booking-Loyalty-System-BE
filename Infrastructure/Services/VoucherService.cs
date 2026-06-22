using Application.DTOs.Voucher;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class VoucherService : IVoucherService
{
    private readonly IApplicationDbContext _context;

    public VoucherService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VoucherTemplateResponse>> GetAvailableVouchersAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Promotions
            .Where(p => p.IsVoucher && p.IsActive && p.StartDate <= now && p.EndDate >= now)
            .Where(p => p.MaxUses == null || p.UsedCount < p.MaxUses)
            .OrderBy(p => p.PointsCost)
            .Select(p => new VoucherTemplateResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DiscountType = p.DiscountType.ToString(),
                DiscountValue = p.DiscountValue,
                PointsCost = p.PointsCost ?? 0,
                MinSpend = p.MinSpend
            })
            .ToListAsync();
    }

    public async Task<CustomerVoucherResponse> RedeemVoucherAsync(Guid userId, Guid promotionId)
    {
        await using var transaction = await _context.BeginTransactionAsync();

        var voucher = await _context.Promotions
            .FirstOrDefaultAsync(p => p.Id == promotionId && p.IsVoucher)
            ?? throw new AppException("Voucher not found.", 404);

        if (!voucher.IsActive)
            throw new AppException("Voucher is not available.", 400);

        var now = DateTime.UtcNow;
        if (now < voucher.StartDate || now > voucher.EndDate)
            throw new AppException("Voucher is not within the valid date range.", 400);

        if (voucher.MaxUses != null && voucher.UsedCount >= voucher.MaxUses)
            throw new AppException("Voucher has reached its redemption limit.", 400);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var pointsCost = voucher.PointsCost ?? 0;
        if (customer.TotalPoints < pointsCost)
            throw new AppException("Insufficient points to redeem this voucher.", 400);

        // Deduct points
        customer.TotalPoints -= pointsCost;

        // Record loyalty transaction
        var ledger = new LoyaltyTransaction
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            Type = LoyaltyTransactionType.Redeem,
            Points = -pointsCost,
            BalanceAfter = customer.TotalPoints,
            Description = $"Redeemed voucher: {voucher.Name}",
            CreatedAt = now
        };
        _context.LoyaltyTransactions.Add(ledger);

        // Create personal voucher (CustomerPromotion)
        var customerPromotion = new CustomerPromotion
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            PromotionId = voucher.Id,
            IsUsed = false,
            ExpiryDate = now.AddDays(30)
        };
        _context.CustomerPromotions.Add(customerPromotion);

        // Increment used count on the voucher template
        voucher.UsedCount++;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new CustomerVoucherResponse
        {
            Id = customerPromotion.Id,
            Name = voucher.Name,
            Description = voucher.Description,
            DiscountType = voucher.DiscountType.ToString(),
            DiscountValue = voucher.DiscountValue,
            MinSpend = voucher.MinSpend,
            ExpiryDate = customerPromotion.ExpiryDate,
            IsUsed = false
        };
    }

    public async Task<List<CustomerVoucherResponse>> GetMyVouchersAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        return await _context.CustomerPromotions
            .Include(cp => cp.Promotion)
            .Where(cp => cp.CustomerId == customer.Id && cp.Promotion.IsVoucher)
            .OrderByDescending(cp => cp.ExpiryDate)
            .Select(cp => new CustomerVoucherResponse
            {
                Id = cp.Id,
                Name = cp.Promotion.Name,
                Description = cp.Promotion.Description,
                DiscountType = cp.Promotion.DiscountType.ToString(),
                DiscountValue = cp.Promotion.DiscountValue,
                MinSpend = cp.Promotion.MinSpend,
                ExpiryDate = cp.ExpiryDate,
                IsUsed = cp.IsUsed
            })
            .ToListAsync();
    }

    public async Task<List<CustomerVoucherResponse>> GetApplicableVouchersAsync(Guid userId, decimal subtotal)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var now = DateTime.UtcNow;

        var vouchers = await _context.CustomerPromotions
            .Include(cp => cp.Promotion)
            .Where(cp => cp.CustomerId == customer.Id
                && cp.Promotion.IsVoucher
                && !cp.IsUsed
                && (cp.ExpiryDate == null || cp.ExpiryDate > now)
                && (cp.Promotion.MinSpend == null || cp.Promotion.MinSpend <= subtotal))
            .Select(cp => new CustomerVoucherResponse
            {
                Id = cp.Id,
                Name = cp.Promotion.Name,
                Description = cp.Promotion.Description,
                DiscountType = cp.Promotion.DiscountType.ToString(),
                DiscountValue = cp.Promotion.DiscountValue,
                MinSpend = cp.Promotion.MinSpend,
                ExpiryDate = cp.ExpiryDate,
                IsUsed = cp.IsUsed
            })
            .ToListAsync();

        // Sort by best discount descending
        return vouchers
            .OrderByDescending(v => v.DiscountType == "FixedAmount"
                ? v.DiscountValue
                : subtotal * v.DiscountValue / 100)
            .ToList();
    }
}
