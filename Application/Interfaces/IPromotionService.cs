using Application.DTOs.Promotion;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPromotionService
{
    // Catalog management
    Task<List<PromotionResponse>> GetAllAsync();

    /// <summary>
    /// Customer-facing list: only promotions that are active, currently within their
    /// start/end window, and not yet exhausted. When <paramref name="userId"/> is supplied,
    /// the list is further filtered to what THIS customer can actually use right now
    /// (đúng hạng + đúng tháng sinh nhật). Điều kiện chi nhánh/địa chỉ chỉ kiểm tra lúc đặt
    /// (Preview/Apply) vì lúc duyệt list chưa biết khách chọn chi nhánh nào.
    /// </summary>
    Task<List<PromotionResponse>> GetActiveAsync(Guid? userId = null);
    Task<PromotionResponse?> GetByIdAsync(Guid id);
    Task<PromotionResponse> CreateAsync(CreatePromotionRequest request);
    Task<PromotionResponse> UpdateAsync(Guid id, UpdatePromotionRequest request);
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Read-only validation + discount calculation for the frontend to preview, enforcing
    /// all eligibility rules for the authenticated customer (sinh nhật / hạng / chi nhánh).
    /// </summary>
    Task<PromotionPreviewResponse> PreviewAsync(string code, decimal subtotal, Guid userId, Guid? branchId = null);

    /// <summary>
    /// Validates a code against a subtotal + the customer's eligibility (sinh nhật / hạng /
    /// chi nhánh) and, on success, increments the promotion's UsedCount on the tracked entity
    /// (caller's SaveChanges/transaction persists it). Returns the promotion id and the
    /// discount to apply. Throws AppException on failure.
    /// </summary>
    Task<(Guid PromotionId, decimal DiscountAmount)> ApplyAsync(string code, decimal subtotal, Customer customer, Guid branchId);
}
