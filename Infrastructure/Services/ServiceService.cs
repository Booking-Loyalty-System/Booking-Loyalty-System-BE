using Application.DTOs.Service;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ServiceService : IServiceService
{
    private readonly IApplicationDbContext _context;

    public ServiceService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceResponse>> GetAllActiveServicesAsync()
    {
        return await _context.Services
            .Where(s => s.IsActive)
            .Include(s => s.Features.OrderBy(f => f.SortOrder))
            .OrderBy(s => s.BasePrice)
            .Select(s => new ServiceResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                BasePrice = s.BasePrice,
                DurationMinutes = s.DurationMinutes,
                Features = s.Features.OrderBy(f => f.SortOrder).Select(f => f.FeatureDescription).ToList()
            })
            .ToListAsync();
    }
}
