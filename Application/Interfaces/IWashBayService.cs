using Application.DTOs.WashBay;

namespace Application.Interfaces;

public interface IWashBayService
{
    Task<List<WashBayResponse>> GetAllAsync();
    Task<List<WashBayResponse>> GetAllAsync(Guid branchId);
    Task<WashBayResponse?> GetByIdAsync(Guid id);
    Task<WashBayResponse> CreateAsync(CreateWashBayRequest request);
    Task<WashBayResponse> UpdateAsync(Guid id, UpdateWashBayRequest request);
    Task DeleteAsync(Guid id);
}
