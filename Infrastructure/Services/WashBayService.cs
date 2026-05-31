using Application.DTOs.WashBay;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class WashBayService : IWashBayService
{
    private readonly IApplicationDbContext _context;

    public WashBayService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WashBayResponse>> GetAllAsync()
    {
        var bays = await _context.WashBays
            .OrderBy(wb => wb.Name)
            .ToListAsync();

        return bays.Select(MapToResponse).ToList();
    }

    public async Task<WashBayResponse?> GetByIdAsync(Guid id)
    {
        var bay = await _context.WashBays.FindAsync(id);
        return bay == null ? null : MapToResponse(bay);
    }

    public async Task<WashBayResponse> CreateAsync(CreateWashBayRequest request)
    {
        var bay = new WashBay
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Status = WashBayStatus.Available,
            SupportedTypes = string.Join(",", request.SupportedTypes),
            CreatedAt = DateTime.UtcNow
        };

        _context.WashBays.Add(bay);
        await _context.SaveChangesAsync();

        return MapToResponse(bay);
    }

    public async Task<WashBayResponse> UpdateAsync(Guid id, UpdateWashBayRequest request)
    {
        var bay = await _context.WashBays.FindAsync(id)
            ?? throw new AppException("Wash bay not found.", 404);

        if (request.Name != null) bay.Name = request.Name;
        if (request.Status != null) bay.Status = Enum.Parse<WashBayStatus>(request.Status);
        if (request.SupportedTypes != null) bay.SupportedTypes = string.Join(",", request.SupportedTypes);

        await _context.SaveChangesAsync();

        return MapToResponse(bay);
    }

    public async Task DeleteAsync(Guid id)
    {
        var bay = await _context.WashBays.FindAsync(id)
            ?? throw new AppException("Wash bay not found.", 404);

        _context.WashBays.Remove(bay);
        await _context.SaveChangesAsync();
    }

    private static WashBayResponse MapToResponse(WashBay bay)
    {
        return new WashBayResponse
        {
            Id = bay.Id,
            Name = bay.Name,
            Status = bay.Status.ToString(),
            SupportedTypes = bay.SupportedTypes.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            CreatedAt = bay.CreatedAt
        };
    }
}
