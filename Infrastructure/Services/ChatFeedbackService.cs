using Application.DTOs.ChatFeedback;
using Application.DTOs.ChatMessage;
using Application.DTOs.Feedback;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ChatFeedbackService : IChatFeedbackService
    {
        private readonly IApplicationDbContext _context;

        public ChatFeedbackService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateChatFeedbackAsync(CreateChatFeedbackRequest request)
        {
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new AppException("Điểm đánh giá phải nằm trong khoảng từ 1 đến 5 sao.", 400);
            }

            var chatSession = await _context.ChatSessions
                .FirstOrDefaultAsync(cs => cs.Id == request.ChatSessionId)
                ?? throw new AppException("Không tìm thấy phiên trò chuyện này.", 404);

            var isFeedbackExist = await _context.ChatFeedbacks
                .AnyAsync(cf => cf.ChatSessionId == request.ChatSessionId);

            if (isFeedbackExist)
            {
                throw new AppException("Phiên trò chuyện này đã được đánh giá trước đó.", 409);
            }

            var chatFeedback = new ChatFeedback
            {
                Id = Guid.NewGuid(),
                ChatSessionId = request.ChatSessionId,
                Rating = request.Rating,
                Comment = request.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatFeedbacks.Add(chatFeedback);
            var result = await _context.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<IEnumerable<ChatFeedbackResponse>> GetLatestChatFeedbacksAsync(int count = 10)
        {
            return await _context.ChatFeedbacks
                .Include(cf => cf.ChatSession)
                    .ThenInclude(cs => cs.Customer)
                .Include(cf => cf.ChatSession)
                    .ThenInclude(cs => cs.Staff)
                .OrderByDescending(cf => cf.CreatedAt)
                .Take(count)
                .Select(cf => new ChatFeedbackResponse
                {
                    Id = cf.Id,
                    ChatSessionId = cf.ChatSessionId,
                    CustomerName = cf.ChatSession.Customer.FullName ?? "Khách hàng",
                    StaffName = cf.ChatSession.Staff.FullName ?? "Nhân viên",
                    Rating = cf.Rating,
                    Comment = cf.Comment,
                    CreatedAt = cf.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ChatStaffStatisticResponse> GetTopChatStaffAsync(int topCount = 5)
        {
            var chatRatings = await _context.ChatFeedbacks
                .Include(cf => cf.ChatSession)
                .Where(cf => cf.ChatSession.StaffId.HasValue)
                .GroupBy(cf => new { cf.ChatSession.StaffId, cf.ChatSession.Staff!.FullName })
                .Select(g => new StaffRatingResponse
                {
                    StaffId = g.Key.StaffId!.Value,
                    StaffName = g.Key.FullName,
                    AverageRating = g.Average(cf => cf.Rating),
                    TotalFeedbacks = g.Count()
                })
                .ToListAsync();

            return new ChatStaffStatisticResponse
            {
                TopChatStaffs = chatRatings.OrderByDescending(s => s.AverageRating).Take(topCount).ToList(),
                LowestChatStaffs = chatRatings.OrderBy(s => s.AverageRating).Take(topCount).ToList()
            };
        }

        public async Task<ChatFeedbackDetailResponse> GetChatFeedbackDetailAsync(Guid feedbackId)
        {
            var feedback = await _context.ChatFeedbacks
                .Include(cf => cf.ChatSession)
                    .ThenInclude(cs => cs.Customer)
                .Include(cf => cf.ChatSession)
                    .ThenInclude(cs => cs.Staff)
                .Include(cf => cf.ChatSession)
                    .ThenInclude(cs => cs.ChatMessages.OrderBy(m => m.CreatedAt)) // Load toàn bộ tin nhắn theo thứ tự thời gian
                .FirstOrDefaultAsync(cf => cf.Id == feedbackId)
                ?? throw new AppException("Không tìm thấy đánh giá cuộc trò chuyện này.", 404);

            return new ChatFeedbackDetailResponse
            {
                FeedbackId = feedback.Id,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt,
                ChatSessionId = feedback.ChatSessionId,
                CustomerName = feedback.ChatSession.Customer?.FullName ?? "Khách hàng",
                CustomerId = feedback.ChatSession.CustomerId, // Cần để tặng Voucher sau này
                StaffName = feedback.ChatSession.Staff?.FullName ?? "Nhân viên/AI",
                StaffId = feedback.ChatSession.StaffId,

                // Map danh sách tin nhắn sang DTO
                Messages = feedback.ChatSession.ChatMessages.Select(m => new ChatMessageResponse
                {
                    Id = m.Id,
                    SenderType = m.SenderType,
                    Message = m.Message,
                    SentAt = m.CreatedAt
                }).ToList()
            };
        }
    }
}