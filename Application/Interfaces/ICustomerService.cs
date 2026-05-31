using Application.DTOs.Customer;

namespace Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerProfileResponse> GetProfileAsync(Guid userId);
    Task<CustomerProfileResponse> UpdateProfileAsync(Guid userId, UpdateCustomerProfileRequest request);
}
