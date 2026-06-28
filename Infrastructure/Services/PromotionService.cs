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
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return promotions.Select(MapToResponse).ToList();
    }

    public async Task<List<PromotionResponse>> GetActiveAsync()
    {
        var now = DateTime.UtcNow;

        var promotions = await _context.Promotions
            .Where(p => p.IsActive
                && p.StartDate <= now
                && p.EndDate >= now
                && (p.MaxUses == null || p.UsedCount < p.MaxUses))
            .OrderByDescending(p => p.PriorityLevel)
            .ThenByDescending(p => p.CreatedAt)
            .ToListAsync();

        return promotions.Select(MapToResponse).ToList();
    }

    public async Task<PromotionResponse?> GetByIdAsync(Guid id)
    {
        var promotion = await _context.Promotions.FindAsync(id);
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
            Description = request.Description,
            DiscountType = Enum.Parse<DiscountType>(request.DiscountType, ignoreCase: true),
            DiscountValue = request.DiscountValue,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxUses = request.MaxUses,
            UsedCount = 0,
            MinSpend = request.MinSpend,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return MapToResponse(promotion);
    }

    public async Task<PromotionResponse> UpdateAsync(Guid id, UpdatePromotionRequest request)
    {
        var promotion = await _context.Promotions.FindAsync(id)
            ?? throw new AppException("Promotion not found.", 404);

        if (request.Description != null) promotion.Description = request.Description;
        if (request.DiscountType != null) promotion.DiscountType = Enum.Parse<DiscountType>(request.DiscountType, ignoreCase: true);
        if (request.DiscountValue.HasValue) promotion.DiscountValue = request.DiscountValue.Value;
        if (request.StartDate.HasValue) promotion.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) promotion.EndDate = request.EndDate.Value;
        if (request.MaxUses.HasValue) promotion.MaxUses = request.MaxUses.Value;
        if (request.MinSpend.HasValue) promotion.MinSpend = request.MinSpend.Value;
        if (request.IsActive.HasValue) promotion.IsActive = request.IsActive.Value;

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

    public async Task<PromotionPreviewResponse> PreviewAsync(string code, decimal subtotal)
    {
        var promotion = await LoadValidPromotionAsync(code, subtotal);
        var discount = ComputeDiscount(promotion, subtotal);

        return new PromotionPreviewResponse
        {
            Code = promotion.Code,
            Subtotal = subtotal,
            DiscountAmount = discount,
            FinalAmount = subtotal - discount
        };
    }

    public async Task<(Guid PromotionId, decimal DiscountAmount)> ApplyAsync(string code, decimal subtotal)
    {
        var promotion = await LoadValidPromotionAsync(code, subtotal);
        var discount = ComputeDiscount(promotion, subtotal);

        // Reserve one use. Caller's SaveChanges/transaction commits this together with the booking.
        promotion.UsedCount += 1;

        return (promotion.Id, discount);
    }

    /// <summary>Loads a promotion by code and enforces all eligibility rules, or throws.</summary>
    private async Task<Promotion> LoadValidPromotionAsync(string code, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new AppException("Promotion code is required.", 400);

        var normalized = code.Trim().ToUpperInvariant();
        var promotion = await _context.Promotions
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
        CreatedAt = p.CreatedAt
    };
}
