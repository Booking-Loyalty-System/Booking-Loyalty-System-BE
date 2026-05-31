using Application.DTOs.Vehicle;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class VehicleService : IVehicleService
{
    private readonly IApplicationDbContext _context;

    public VehicleService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleResponse> AddVehicleAsync(Guid userId, AddVehicleRequest request)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        // Check max 5 vehicles
        var vehicleCount = await _context.Vehicles
            .CountAsync(v => v.CustomerId == customer.Id && !v.IsDeleted);

        if (vehicleCount >= 5)
            throw new AppException("Maximum 5 vehicles allowed per customer.", 400);

        // Check duplicate license plate
        var existingPlate = await _context.Vehicles
            .AnyAsync(v => v.LicensePlate == request.LicensePlate && !v.IsDeleted);

        if (existingPlate)
            throw new AppException("A vehicle with this license plate already exists.", 409);

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            LicensePlate = request.LicensePlate,
            Type = Enum.Parse<VehicleType>(request.VehicleType),
            VehicleName = request.VehicleName,
            Brand = request.Brand,
            Model = request.Model,
            Color = request.Color,
            IsPrimary = request.IsPrimary || vehicleCount == 0,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return MapToResponse(vehicle);
    }

    public async Task<List<VehicleResponse>> GetMyVehiclesAsync(Guid userId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var vehicles = await _context.Vehicles
            .Where(v => v.CustomerId == customer.Id && !v.IsDeleted)
            .OrderByDescending(v => v.IsPrimary)
            .ThenByDescending(v => v.CreatedAt)
            .ToListAsync();

        return vehicles.Select(MapToResponse).ToList();
    }

    public async Task DeleteVehicleAsync(Guid userId, Guid vehicleId)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == vehicleId && v.CustomerId == customer.Id && !v.IsDeleted)
            ?? throw new AppException("Vehicle not found.", 404);

        // Check if it's the last vehicle
        var vehicleCount = await _context.Vehicles
            .CountAsync(v => v.CustomerId == customer.Id && !v.IsDeleted);

        if (vehicleCount <= 1)
            throw new AppException("Cannot delete your last vehicle.", 400);

        // Check no active bookings
        var activeStatuses = new[] { BookingStatus.Confirmed, BookingStatus.InProgress };
        var hasActiveBookings = await _context.Bookings
            .AnyAsync(b => b.VehicleId == vehicleId && activeStatuses.Contains(b.Status));

        if (hasActiveBookings)
            throw new AppException("Cannot delete vehicle with active bookings.", 400);

        vehicle.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    private static VehicleResponse MapToResponse(Vehicle vehicle)
    {
        return new VehicleResponse
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Type = vehicle.Type.ToString(),
            IsPrimary = vehicle.IsPrimary,
            VehicleName = vehicle.VehicleName,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Color = vehicle.Color,
            CreatedAt = vehicle.CreatedAt
        };
    }
}
