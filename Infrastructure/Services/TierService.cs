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
            .ToListAsync();

        return tiers.Select(MapToResponse).ToList();
    }

    public async Task<TierResponse> GetTierByIdAsync(Guid id)
    {
        var tier = await _context.Tiers
            .FirstOrDefaultAsync(t => t.Id == id)
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

        var tier = new Tier
        {
            Id = Guid.NewGuid(),
            TierName = request.TierName,
            PointRate = request.PointRate,
            BookingWindow = request.BookingWindow,
            Level = Enum.Parse<PriorityLevel>(request.Level) // Parse giống VehicleType
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