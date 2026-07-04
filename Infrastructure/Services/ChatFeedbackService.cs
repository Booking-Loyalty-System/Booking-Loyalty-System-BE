using Application.Common;
using Application.DTOs.ChatFeedback;
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
    }
}