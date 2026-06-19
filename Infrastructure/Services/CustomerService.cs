using Application.DTOs.Customer;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly IApplicationDbContext _context;

    public CustomerService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerProfileResponse> GetProfileAsync(Guid userId)
    {
        var customer = await _context.Customers
            .Include(c => c.User)
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        return MapToResponse(customer);
    }

    public async Task<CustomerProfileResponse> UpdateProfileAsync(Guid userId, UpdateCustomerProfileRequest request)
    {
        var customer = await _context.Customers
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new AppException("Customer profile not found.", 404);

        if (request.FullName != null) customer.FullName = request.FullName;
        if (request.PhoneNumber != null) customer.PhoneNumber = request.PhoneNumber;
        if (request.DateOfBirth.HasValue) customer.DateOfBirth = request.DateOfBirth;

        await _context.SaveChangesAsync();

        return MapToResponse(customer);
    }

    private static CustomerProfileResponse MapToResponse(Domain.Entities.Customer customer)
    {
        return new CustomerProfileResponse
        {
            Id = customer.Id,
            Email = customer.User.Email,
            FullName = customer.FullName,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            Tier = customer.Tier.TierName,
            TotalPoints = customer.TotalPoints,
            TotalWashes = customer.TotalWashes,
            TotalSpent = customer.TotalSpent,
            CreatedAt = customer.CreatedAt
        };
    }
}
