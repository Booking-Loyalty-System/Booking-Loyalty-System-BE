using Application.DTOs.Tier;

namespace Application.Interfaces;

public interface ITierService
{
    Task<List<TierResponse>> GetAllAsync();
    Task<CustomerTierResponse> GetMyTierAsync(Guid userId);
}
