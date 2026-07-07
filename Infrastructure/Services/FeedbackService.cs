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

        public async Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid userId, FeedbackRequest request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserId == userId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found ");
            }
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.CustomerId == customer.Id);

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
                CustomerId = customer.Id,
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
                .Where(c => c.Id == customer.Id)
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

        public async Task<List<FeedbackFilterResponse>> GetFeedbacksAsync(bool isDescending = true)
        {
            var query = _context.Feedbacks
                .Include(f => f.Booking)
                .AsNoTracking();

            if (isDescending)
            {
                query = query.OrderByDescending(f => (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0);
            }
            else
            {
                query = query.OrderBy(f => (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0);
            }

            return await query.Select(f => new FeedbackFilterResponse
            {
                Id = f.Id,
                BookingCode = f.Booking.BookingCode,
                OverallRating = (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            }).ToListAsync();
        }

        public async Task<FeedbackStatisticsResponse> GetFeedbackStatisticsAsync(int topCount = 5)
        {
            var response = new FeedbackStatisticsResponse();

            var staffRatings = await _context.Bookings
                .Where(b => b.StaffId.HasValue && b.Feedback != null)
                .GroupBy(b => new { b.StaffId, b.Staff!.FullName })
                .Select(g => new StaffRatingResponse
                {
                    StaffId = g.Key.StaffId!.Value,
                    StaffName = g.Key.FullName,
                    AverageRating = g.Average(b => b.Feedback!.StaffRating),
                    TotalFeedbacks = g.Count()
                })
                .ToListAsync();

            response.TopStaffs = staffRatings.OrderByDescending(s => s.AverageRating).Take(topCount).ToList();
            response.LowestStaffs = staffRatings.OrderBy(s => s.AverageRating).Take(topCount).ToList();

            var serviceRatings = await _context.Bookings
                .Where(b => b.Feedback != null)
                .GroupBy(b => new { b.WashPackageId, b.WashPackage.Name }) // Dựa theo Booking trỏ đến WashPackage
                .Select(g => new ServiceRatingResponse
                {
                    ServiceId = g.Key.WashPackageId,
                    ServiceName = g.Key.Name,
                    AverageRating = g.Average(b => b.Feedback!.ServiceRating),
                    TotalFeedbacks = g.Count()
                })
                .ToListAsync();

            response.TopServices = serviceRatings.OrderByDescending(s => s.AverageRating).Take(topCount).ToList();
            response.LowestServices = serviceRatings.OrderBy(s => s.AverageRating).Take(topCount).ToList();

            // 3. Thống kê Staff Chat Feedback
            // Giả định ChatSession có thuộc tính StaffId và điều hướng tới Staff
            var chatRatings = await _context.ChatFeedbacks
                .Include(cf => cf.ChatSession)
                .Where(cf => cf.ChatSession.StaffId.HasValue)
                .GroupBy(cf => new { cf.ChatSession.StaffId, cf.ChatSession.Staff!.FullName })
                .Select(g => new StaffRatingResponse
                {
                    StaffId = g.Key.StaffId!.Value,
                    StaffName = g.Key.FullName,
                    AverageRating = g.Average(cf => cf.Rating), // Dựa theo thuộc tính Rating của ChatFeedback
                    TotalFeedbacks = g.Count()
                })
                .ToListAsync();

            response.TopChatStaffs = chatRatings.OrderByDescending(s => s.AverageRating).Take(topCount).ToList();
            response.LowestChatStaffs = chatRatings.OrderBy(s => s.AverageRating).Take(topCount).ToList();

            return response;
        }
    }
}
