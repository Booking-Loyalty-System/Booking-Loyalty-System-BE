using Application.DTOs.Feedback;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IApplicationDbContext _context;

        public FeedbackService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid customerId, FeedbackRequest request)
        {
            // 1. Kiểm tra đơn đặt lịch hợp lệ (Đổi dto thành request)
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.CustomerId == customerId);

            if (booking == null)
                throw new KeyNotFoundException("Không tìm thấy đơn đặt lịch hợp lệ của bạn.");

            // 2. Đảm bảo đơn này chưa từng được đánh giá
            var isFeedbacked = await _context.Feedbacks.AnyAsync(f => f.BookingId == request.BookingId);
            if (isFeedbacked)
                throw new InvalidOperationException("Đơn đặt lịch này đã được bạn gửi đánh giá trước đó.");

            // 3. Khởi tạo thực thể theo cấu trúc mới
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                BookingId = request.BookingId,
                CustomerId = customerId,
                StaffRating = request.StaffRating,
                ServiceRating = request.ServiceRating,
                PriceRating = request.PriceRating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            // 4. Lấy thông tin hiển thị lên DTO phản hồi
            var customerName = await _context.Customers
                .Where(c => c.Id == customerId)
                .Select(c => c.FullName)
                .FirstOrDefaultAsync() ?? "Khách hàng";

            // Khởi tạo FeedbackModerationResponse bằng Object Initializer { }
            return new FeedbackResponse
            {
                BookingCode = booking.BookingCode,
                CustomerName = customerName,
                StaffRating = feedback.StaffRating,
                ServiceRating = feedback.ServiceRating,
                PriceRating = feedback.PriceRating,
                OverallRating = feedback.OverallRating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt
                // Huy có thể bổ sung trường StaffReply = null, StaffName = null nếu DTO yêu cầu nhé
            };
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync()
        {
            return await _context.Feedbacks
           .Include(f => f.Booking)
           .ThenInclude(b => b.Customer)
           .OrderByDescending(f => f.CreatedAt)
           .Select(f => new FeedbackResponse
           {
               BookingCode = f.Booking.BookingCode,
               CustomerName = f.Booking.Customer.FullName,
               StaffRating = f.StaffRating,
               ServiceRating = f.ServiceRating,
               PriceRating = f.PriceRating,
               OverallRating = f.OverallRating,
               Comment = f.Comment,
               CreatedAt = f.CreatedAt
           })
           .ToListAsync();
        }

        /*public async Task<FeedbackModerationResponse> StaffReplyFeedbackAsync(Guid userId, Guid feedbackId, ReplyFeedbackRequest request)
        {
            // 1. Tìm thông tin Staff dựa vào UserId của tài khoản đang đăng nhập
            var staff = await _context.Staffs
                .FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new AppException("Staff not found.", 404);

            // 2. Tìm bản ghi Feedback cần trả lời
            var feedback = await _context.Feedbacks
                .Include(f => f.Booking)
                .Include(f => f.Customer)
                .FirstOrDefaultAsync(f => f.Id == feedbackId)
                ?? throw new KeyNotFoundException("Không tìm thấy bài đánh giá này.");

            // 3. Cập nhật thông tin phản hồi (Giả định Entity Feedback của bạn có các trường này)
            // Nếu Entity chưa có, Huy mở file Feedback.cs thêm: public string? StaffReply { get; set; } ...
            feedback.StaffReply = request.Content; // Hoặc request.StaffReply tùy DTO của bạn
            feedback.RepliedAt = DateTime.UtcNow;
            feedback.RepliedByStaffId = staff.Id;

            await _context.SaveChangesAsync();

            // 4. Trả về đúng kiểu FeedbackModerationResponse
            return new FeedbackModerationResponse
            {
                BookingCode = feedback.Booking.BookingCode,
                CustomerName = feedback.Customer.FullName,
                StaffRating = feedback.StaffRating,
                ServiceRating = feedback.ServiceRating,
                PriceRating = feedback.PriceRating,
                OverallRating = feedback.OverallRating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt
                // Nếu FeedbackModerationResponse có chứa thông tin phản hồi, Huy gán thêm tại đây:
                // StaffReply = feedback.StaffReply,
                // StaffName = staff.FullName
            };
        }*/
    }
}
