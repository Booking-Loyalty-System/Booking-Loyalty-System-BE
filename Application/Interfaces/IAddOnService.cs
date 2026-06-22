using Application.DTOs.AddOn;

namespace Application.Interfaces;

public interface IAddOnService
{
    Task<List<AddOnResponse>> GetAllAsync(bool activeOnly);
    Task<AddOnResponse?> GetByIdAsync(Guid id);
    Task<AddOnResponse> CreateAsync(CreateAddOnRequest request);
    Task<AddOnResponse> UpdateAsync(Guid id, UpdateAddOnRequest request);
    Task DeleteAsync(Guid id);
}
