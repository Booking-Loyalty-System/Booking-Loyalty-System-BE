using Application.DTOs.Store;

namespace Application.Interfaces;

public interface IStoreService
{
    Task<List<StoreResponse>> GetAllActiveStoresAsync();
}
