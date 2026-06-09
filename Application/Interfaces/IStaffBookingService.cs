using Application.DTOs.Staff;

namespace Application.Interfaces;

public interface IStaffBookingService
{
    Task<List<StaffBookingResponse>> GetTodayBookingsAsync(Guid branchId);
    Task<StaffBookingResponse> SearchByBookingCodeAsync(string bookingCode);
    Task<StaffBookingResponse> ConfirmBookingAsync(Guid bookingId);
    Task<StaffBookingResponse> StartWashAsync(Guid bookingId);
    Task<StaffBookingResponse> CompleteWashAsync(Guid bookingId);
    Task<StaffBookingResponse> CancelBookingAsync(Guid bookingId, string? reason);
}
