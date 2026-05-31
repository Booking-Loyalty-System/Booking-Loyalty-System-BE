using Application.DTOs.Store;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StoreService : IStoreService
{
    private readonly IApplicationDbContext _context;

    public StoreService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StoreResponse>> GetAllActiveStoresAsync()
    {
        return await _context.Stores
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new StoreResponse
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                City = s.City,
                OpenTime = s.OpenTime,
                CloseTime = s.CloseTime,
                SlotCapacity = s.SlotCapacity
            })
            .ToListAsync();
    }
}
