using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR; 
using Infrastructure.Hubs;       
namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<BookingHub> _hubContext; // Thêm biến này

    public NotificationService(ApplicationDbContext context, IHubContext<BookingHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext; // Gán giá trị
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateNotificationAsync(Guid userId, string title, string message, string type)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type, // VD: "Booking", "Loyalty", "System"
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
    
    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }
    
    public async Task<(List<Notification> Items, int TotalCount)> GetNotificationsPagedAsync(Guid userId, int page, int pageSize)
    {
        var query = _context.Notifications.Where(n => n.UserId == userId);
    
        int totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    
    public async Task SendNotificationToStaffAsync(Guid branchId, string title, string message, Guid relatedId, string type)
    {
        // Kiểm tra xem Staff có tồn tại không
        var staffIds = await _context.Staffs
            .Where(s => s.BranchId == branchId) // Kiểm tra lại tên property ở đây
            .Select(s => s.UserId)
            .ToListAsync();

        var notifications = staffIds.Select(userId => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            ReferenceId = relatedId,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        foreach (var userId in staffIds)
        {
            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", new { title, message, relatedId, type });
        }
    }
    
    public async Task SendNotificationToCustomerAsync(Guid customerId, string title, string message, Guid? relatedId, string type)
    {
        // 1. Tìm thông tin Customer để lấy ra UserId phục vụ cho SignalR và lưu bảng Notification
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customerId);
            
        if (customer == null) return;

        // 2. Tạo thực thể thông báo mới
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = customer.UserId, 
            Title = title,
            Message = message,
            ReferenceId = relatedId, 
            Type = type,        
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Lưu vào Database
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // 4. Bắn SignalR Realtime đến client của Customer
        await _hubContext.Clients.User(customer.UserId.ToString())
            .SendAsync("ReceiveNotification", new { title, message, relatedId, type });
    }
}