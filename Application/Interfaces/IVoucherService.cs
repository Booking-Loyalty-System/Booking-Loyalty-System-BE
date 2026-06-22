using Application.DTOs.Voucher;

namespace Application.Interfaces;

public interface IVoucherService
{
    Task<List<VoucherTemplateResponse>> GetAvailableVouchersAsync();
    Task<CustomerVoucherResponse> RedeemVoucherAsync(Guid userId, Guid promotionId);
    Task<List<CustomerVoucherResponse>> GetMyVouchersAsync(Guid userId);
    Task<List<CustomerVoucherResponse>> GetApplicableVouchersAsync(Guid userId, decimal subtotal);
}
