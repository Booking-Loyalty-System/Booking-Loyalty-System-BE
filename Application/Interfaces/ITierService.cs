using Application.DTOs.Tier;

namespace Application.Interfaces;

public interface ITierService
{
    Task<List<TierResponse>> GetAllTiersAsync();
    Task<TierResponse> GetTierByIdAsync(Guid id);
    Task<TierResponse> CreateTierAsync(CreateTierRequest request);
    Task<TierResponse> UpdateTierAsync(Guid id, UpdateTierRequest request);
    Task DeleteTierAsync(Guid id);
}