using Application.DTOs.AddOn;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AddOnService : IAddOnService
{
    private readonly IApplicationDbContext _context;

    public AddOnService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AddOnResponse>> GetAllAsync(bool activeOnly)
    {
        var query = _context.AddOns.AsQueryable();
        if (activeOnly)
            query = query.Where(a => a.IsActive);

        var addOns = await query.OrderBy(a => a.Name).ToListAsync();
        return addOns.Select(MapToResponse).ToList();
    }

    public async Task<AddOnResponse?> GetByIdAsync(Guid id)
    {
        var addOn = await _context.AddOns.FindAsync(id);
        return addOn == null ? null : MapToResponse(addOn);
    }

    public async Task<AddOnResponse> CreateAsync(CreateAddOnRequest request)
    {
        var addOn = new AddOn
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DurationMinutes = request.DurationMinutes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.AddOns.Add(addOn);
        await _context.SaveChangesAsync();

        return MapToResponse(addOn);
    }

    public async Task<AddOnResponse> UpdateAsync(Guid id, UpdateAddOnRequest request)
    {
        var addOn = await _context.AddOns.FindAsync(id)
            ?? throw new AppException("Add-on not found.", 404);

        if (request.Name != null) addOn.Name = request.Name;
        if (request.Description != null) addOn.Description = request.Description;
        if (request.Price.HasValue) addOn.Price = request.Price.Value;
        if (request.DurationMinutes.HasValue) addOn.DurationMinutes = request.DurationMinutes.Value;
        if (request.IsActive.HasValue) addOn.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();

        return MapToResponse(addOn);
    }

    public async Task DeleteAsync(Guid id)
    {
        var addOn = await _context.AddOns.FindAsync(id)
            ?? throw new AppException("Add-on not found.", 404);

        _context.AddOns.Remove(addOn);
        await _context.SaveChangesAsync();
    }

    private static AddOnResponse MapToResponse(AddOn addOn)
    {
        return new AddOnResponse
        {
            Id = addOn.Id,
            Name = addOn.Name,
            Description = addOn.Description,
            Price = addOn.Price,
            DurationMinutes = addOn.DurationMinutes,
            IsActive = addOn.IsActive,
            CreatedAt = addOn.CreatedAt
        };
    }
}
