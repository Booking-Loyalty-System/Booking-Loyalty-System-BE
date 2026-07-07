using Application.DTOs.Branch;
using Application.DTOs.TimeSlot;

namespace Application.Interfaces;

public interface IBranchService
{
    Task<List<BranchResponse>> GetAllAsync();
    Task<BranchResponse?> GetByIdAsync(Guid id);
    Task<BranchResponse> CreateAsync(CreateBranchRequest request);
    Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request);
    Task DeleteAsync(Guid id);

    // New: Configure branch time slots
    Task SetupBranchTimeSlotsAsync(Guid branchId, List<TimeSlotConfigDto> configs);
}
