using Domain.Entities;

namespace Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task<int> GetUnreadCountAsync(Guid userId);
}