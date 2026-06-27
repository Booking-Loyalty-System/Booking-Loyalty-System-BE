using Domain.Entities;

namespace Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task CreateNotificationAsync(Guid userId, string title, string message, string type);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task<(List<Notification> Items, int TotalCount)> GetNotificationsPagedAsync(Guid userId, int page, int pageSize);
    Task SendNotificationToStaffAsync(Guid branchId, string title, string message, Guid relatedId, string type);
    Task SendNotificationToCustomerAsync(Guid customerId, string title, string message, Guid? relatedId, string type);
}