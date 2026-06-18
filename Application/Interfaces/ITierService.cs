using Application.DTOs.Tier;

namespace Application.Interfaces;

public interface ITierService
{
    Task<List<TierResponse>> GetAllAsync();
    Task<CustomerTierResponse> GetMyTierAsync(Guid userId);
    Task<TierResponse> GetByIdAsync(Guid id);
    Task<TierResponse> CreateAsync(CreateTierRequest request);
    Task<TierResponse> UpdateAsync(Guid id, UpdateTierRequest request);
    Task DeleteAsync(Guid id);
}
