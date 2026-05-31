using Application.DTOs.Service;

namespace Application.Interfaces;

public interface IServiceService
{
    Task<List<ServiceResponse>> GetAllActiveServicesAsync();
}
