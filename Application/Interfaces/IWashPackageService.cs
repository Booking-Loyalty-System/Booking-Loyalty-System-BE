using Application.DTOs.WashPackage;

namespace Application.Interfaces;

public interface IWashPackageService
{
    Task<List<WashPackageResponse>> GetAllActiveAsync();
    Task<WashPackageResponse?> GetByIdAsync(Guid id);
    Task<List<WashPackageResponse>> GetAllAsync();
    Task<WashPackageResponse> CreateAsync(CreateWashPackageRequest request);
    Task<WashPackageResponse> UpdateAsync(Guid id, UpdateWashPackageRequest request);
    Task DeactivateAsync(Guid id);
}
