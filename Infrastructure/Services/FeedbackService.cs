using Application.DTOs.AI;
using Application.DTOs.Feedback;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public FeedbackService(IApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
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
            };
        }

        /* public async Task<FeedbackResponse> CustomerCreateFeedbackAsync(Guid userId, FeedbackRequest request)
         {
             var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserId == userId);
             if (customer == null) throw new KeyNotFoundException("Customer not found ");
             var booking = await _context.Bookings
                 .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.CustomerId == customer.Id);
             if (booking == null) throw new KeyNotFoundException("Không tìm thấy đơn đặt lịch hợp lệ của bạn.");
             var isFeedbacked = await _context.Feedbacks.AnyAsync(f => f.BookingId == request.BookingId);
             if (isFeedbacked) throw new InvalidOperationException("Đơn đặt lịch này đã được bạn gửi đánh giá trước đó.");
             // --- 1) Gọi moderation API trước khi lưu ---
             if (!string.IsNullOrWhiteSpace(request.Comment))
             {
                 var client = _httpClientFactory.CreateClient("ProfanityApi");
                 var payload = new { text = request.Comment };
                 var json = System.Text.Json.JsonSerializer.Serialize(payload);
                 var content = new StringContent(json, Encoding.UTF8, "application/json");
                 HttpResponseMessage httpResp;
                 try
                 {
                     httpResp = await client.PostAsync("check", content); // endpoint /check
                 }
                 catch (Exception ex)
                 {
                     // tùy chọn: nếu API chết, bạn có thể
                     // - block luôn, hoặc
                     // - cho phép lưu và đánh dấu pending_moderation
                     throw new InvalidOperationException("Không thể kiểm tra nội dung hiện tại, vui lòng thử lại sau.", ex);
                 }
                 if (!httpResp.IsSuccessStatusCode)
                 {
                     throw new InvalidOperationException("Lỗi khi kiểm tra nội dung phản hồi.");
                 }
                 var respJson = await httpResp.Content.ReadAsStringAsync();
                 var checkResult = System.Text.Json.JsonSerializer.Deserialize<ProfanityCheckResponse>(
                     respJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                 if (checkResult?.IsProfane == true)
                 {
                     // Tùy: trả lỗi hoặc lưu trạng thái pending và không công khai
                     throw new InvalidOperationException("Phát hiện ngôn ngữ không phù hợp trong phản hồi.");
                 }
             }
             // --- 2) Nếu qua kiểm tra, mới lưu vào DB ---
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
             var customerName = await _context.Customers
                 .Where(c => c.Id == customer.Id)
                 .Select(c => c.FullName)
                 .FirstOrDefaultAsync() ?? "Khách hàng";
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
             };
         }
 */
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

        public async Task<List<FeedbackFilterResponse>> GetFeedbacksAsync(string? sortBy = "newest", bool? isGiftedFilter = null)
        {
            // 1. Tạo query gốc, Include các bảng cần thiết để lấy dữ liệu map sang DTO
            var query = _context.Feedbacks
                .Include(f => f.Booking)
                .AsNoTracking();

            // 2. FILTER: Lọc theo trạng thái đã đền bù hay chưa
            // Điểm mấu chốt: Dựa vào logic Any() check BookingId và IsGifted ở bảng RewardRedemptions
            if (isGiftedFilter.HasValue)
            {
                if (isGiftedFilter.Value)
                {
                    // Chỉ lấy các feedback của Booking đã được đền bù
                    query = query.Where(f => _context.RewardRedemptions
                        .Any(r => r.BookingId == f.BookingId && r.IsGifted));
                }
                else
                {
                    // Chỉ lấy các feedback của Booking CHƯA được đền bù
                    query = query.Where(f => !_context.RewardRedemptions
                        .Any(r => r.BookingId == f.BookingId && r.IsGifted));
                }
            }

            // 3. SORTING: Sắp xếp theo các tiêu chí (Mới/Cũ, Tệ/Tốt)
            query = sortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(f => f.CreatedAt),

                // Tệ nhất lên trước (Lowest rating)
                "lowest-rating" => query.OrderBy(f => (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0),

                // Tốt nhất lên trước (Highest rating)
                "highest-rating" => query.OrderByDescending(f => (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0),

                // Mặc định là "newest" (Mới nhất lên trước)
                _ => query.OrderByDescending(f => f.CreatedAt)
            };

            // 4. PROJECTION: Trả về dữ liệu map sang FeedbackFilterResponse
            return await query.Select(f => new FeedbackFilterResponse
            {
                Id = f.Id,
                BookingCode = f.Booking.BookingCode,
                OverallRating = (f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt,
                BookingId = f.BookingId,
                CustomerId = f.CustomerId,
                CustomerName = f.Booking.Customer.FullName,
                StaffRating = f.StaffRating,
                ServiceRating = f.ServiceRating,
                PriceRating = f.PriceRating,

                // Logic gán IsGifted hiển thị ra ngoài UI
                IsGifted = _context.RewardRedemptions
                    .Any(r => r.BookingId == f.BookingId && r.IsGifted)

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

            return response;
        }

        public async Task<BranchFeedbackSummaryResponse> GetCustomerFeedbacksAsync(Guid? branchId = null, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Feedbacks.AsQueryable();

            // Nếu người dùng chọn chi nhánh thì lọc theo branchId
            if (branchId.HasValue && branchId.Value != Guid.Empty)
            {
                query = query.Where(f => f.Booking.BranchTimeSlot.BranchId == branchId.Value);
            }

            // 1. Tính tổng số feedback và điểm trung bình (Tất cả hoặc theo Branch)
            var totalCount = await query.CountAsync();
            var avgRating = totalCount > 0
                ? await query.AverageAsync(f => (double)(f.StaffRating + f.ServiceRating + f.PriceRating) / 3.0)
                : 0;

            // 2. Phân trang lấy danh sách feedback
            var feedbacks = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new CustomerFeedbackResponse
                {
                    CustomerName = f.Booking.Customer.FullName,
                    OverallRating = f.OverallRating,
                    StaffRating = f.StaffRating,
                    ServiceRating = f.ServiceRating,
                    PriceRating = f.PriceRating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();

            return new BranchFeedbackSummaryResponse
            {
                AverageRating = Math.Round(avgRating, 1),
                TotalFeedbacks = totalCount,
                Feedbacks = feedbacks
            };
        }
    }
}
