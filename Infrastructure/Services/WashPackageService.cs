using System.Text.Json;
using Application.DTOs.WashPackage;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class WashPackageService : IWashPackageService
{
    private readonly IApplicationDbContext _context;

    public WashPackageService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WashPackageResponse>> GetAllActiveAsync()
    {
        var packages = await _context.WashPackages
            .Where(wp => wp.IsActive)
            .OrderBy(wp => wp.Price)
            .ToListAsync();

        return packages.Select(MapToResponse).ToList();
    }

    public async Task<WashPackageResponse?> GetByIdAsync(Guid id)
    {
        var package = await _context.WashPackages.FindAsync(id);
        return package == null ? null : MapToResponse(package);
    }

    public async Task<List<WashPackageResponse>> GetAllAsync()
    {
        var packages = await _context.WashPackages
            .OrderBy(wp => wp.CreatedAt)
            .ToListAsync();

        return packages.Select(MapToResponse).ToList();
    }

    public async Task<WashPackageResponse> CreateAsync(CreateWashPackageRequest request)
    {
        var package = new WashPackage
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DurationMinutes = request.DurationMinutes,
            Features = request.Features != null ? JsonSerializer.Serialize(request.Features) : null,
            VehicleType = request.VehicleType != null ? Enum.Parse<VehicleType>(request.VehicleType) : null,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.WashPackages.Add(package);
        await _context.SaveChangesAsync();

        return MapToResponse(package);
    }

    public async Task<WashPackageResponse> UpdateAsync(Guid id, UpdateWashPackageRequest request)
    {
        var package = await _context.WashPackages.FindAsync(id)
            ?? throw new AppException("Wash package not found.", 404);

        if (request.Name != null) package.Name = request.Name;
        if (request.Description != null) package.Description = request.Description;
        if (request.Price.HasValue) package.Price = request.Price.Value;
        if (request.DurationMinutes.HasValue) package.DurationMinutes = request.DurationMinutes.Value;
        if (request.Features != null) package.Features = JsonSerializer.Serialize(request.Features);
        if (request.VehicleType != null) package.VehicleType = Enum.Parse<VehicleType>(request.VehicleType);
        if (request.IsActive.HasValue) package.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();

        return MapToResponse(package);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var package = await _context.WashPackages.FindAsync(id)
            ?? throw new AppException("Wash package not found.", 404);

        package.IsActive = false;
        await _context.SaveChangesAsync();
    }

    private static WashPackageResponse MapToResponse(WashPackage package)
    {
        return new WashPackageResponse
        {
            Id = package.Id,
            Name = package.Name,
            Description = package.Description,
            Price = package.Price,
            DurationMinutes = package.DurationMinutes,
            Features = package.Features != null
                ? JsonSerializer.Deserialize<List<string>>(package.Features) ?? new()
                : new(),
            VehicleType = package.VehicleType?.ToString(),
            IsActive = package.IsActive,
            CreatedAt = package.CreatedAt
        };
    }
}
