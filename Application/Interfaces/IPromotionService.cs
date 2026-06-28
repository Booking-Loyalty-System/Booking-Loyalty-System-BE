using Application.DTOs.Promotion;

namespace Application.Interfaces;

public interface IPromotionService
{
    // Catalog management
    Task<List<PromotionResponse>> GetAllAsync();

    /// <summary>
    /// Customer-facing list: only promotions that are active, currently within their
    /// start/end window, and not yet exhausted. For the frontend to browse/show.
    /// </summary>
    Task<List<PromotionResponse>> GetActiveAsync();
    Task<PromotionResponse?> GetByIdAsync(Guid id);
    Task<PromotionResponse> CreateAsync(CreatePromotionRequest request);
    Task<PromotionResponse> UpdateAsync(Guid id, UpdatePromotionRequest request);
    Task DeleteAsync(Guid id);

    /// <summary>Read-only validation + discount calculation for the frontend to preview.</summary>
    Task<PromotionPreviewResponse> PreviewAsync(string code, decimal subtotal);

    /// <summary>
    /// Validates a code against a subtotal and, on success, increments the promotion's
    /// UsedCount on the tracked entity (caller's SaveChanges/transaction persists it).
    /// Returns the promotion id and the discount to apply. Throws AppException on failure.
    /// </summary>
    Task<(Guid PromotionId, decimal DiscountAmount)> ApplyAsync(string code, decimal subtotal);
}
