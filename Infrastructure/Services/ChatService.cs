using Application.DTOs.Chat;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Hubs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PayOS.Exceptions;

namespace Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hub;

        public ChatService(ApplicationDbContext context, IHubContext<ChatHub> hub)
        {
            _context = context;
            _hub = hub;
        }


        // 1. Staff lấy danh sách các phiên chat đang xếp hàng đợi người thật
        public async Task<IEnumerable<ChatSessionResponse>> GetWaitingSessionsAsync()
        {
            return await _context.ChatSessions
                .Include(s => s.Customer)
                .Include(s => s.ChatMessages)
                .Where(s => s.Status == ChatSessionStatus.WaitingForStaff)
                .OrderBy(s => s.UpdatedAt)
                .Select(s => new ChatSessionResponse
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.FullName,
                    Status = s.Status.ToString(),
                    CreatedAt = s.CreatedAt,
                    Messages = s.ChatMessages.OrderBy(m => m.CreatedAt).Select(m => new ChatMessageResponse
                    {
                        Id = m.Id,
                        SenderType = m.SenderType,
                        Message = m.Message,
                        SenderName = m.SenderType == "User" ? s.Customer.FullName : m.SenderType,
                        CreatedAt = m.CreatedAt
                    }).ToList()
                }).ToListAsync();
        }

        // 2. Staff bấm "Tiếp nhận" (Chấp nhận hỗ trợ khách hàng này)
        public async Task<ChatSessionResponse> AcceptChatSessionAsync(Guid userId, Guid sessionId)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new KeyNotFoundException("Tài khoản nhân viên không tồn tại.");

            var session = await _context.ChatSessions
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == sessionId)
                ?? throw new KeyNotFoundException("Không tìm thấy phiên trò chuyện.");

            session.StaffId = staff.Id;
            session.Status = ChatSessionStatus.HandledByStaff;
            session.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ChatSessionResponse
            {
                Id = session.Id,
                CustomerId = session.CustomerId,
                CustomerName = session.Customer.FullName,
                Status = session.Status.ToString(),
                StaffId = staff.Id,
                StaffName = staff.FullName,
                CreatedAt = session.CreatedAt
            };
        }

        // 3. Staff gửi tin nhắn chat trực tiếp với khách
        public async Task<ChatMessageResponse> StaffSendMessageAsync(Guid userId, Guid sessionId, string message)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new KeyNotFoundException("Nhân viên không tồn tại.");

            var session = await _context.ChatSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.Status == ChatSessionStatus.HandledByStaff)
                ?? throw new InvalidOperationException("Phiên chat chưa được tiếp nhận hoặc đã đóng.");

            var msgEntity = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = sessionId,
                CustomerId = session.CustomerId,
                SenderType = "Staff",
                StaffId = staff.Id,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(msgEntity);
            session.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            string safeGroupId = session.Id.ToString().ToLower().Trim();
            Console.WriteLine($"====================================");
            Console.WriteLine($"[STAFF GUI TIN] Dang ban vao Group: '{session.Id.ToString()}'");
            Console.WriteLine($"====================================");
            await _hub.Clients.Group(safeGroupId).SendAsync("ReceiveMessage", new
            {
                chatSessionId = session.Id,
                senderType = msgEntity.SenderType,
                message = msgEntity.Message,
                createdAt = msgEntity.CreatedAt
            });

            return new ChatMessageResponse
            {
                Id = msgEntity.Id,
                SenderType = "Staff",
                Message = msgEntity.Message,
                SenderName = staff.FullName,
                CreatedAt = msgEntity.CreatedAt
            };
        }

        // 4. Hoàn thành tư vấn - Đóng phiên chat (Khách muốn chat lại sẽ tạo phiên mới)
        public async Task<bool> CloseChatSessionAsync(Guid sessionId)
        {
            var session = await _context.ChatSessions.FindAsync(sessionId);
            if (session == null) return false;

            session.Status = ChatSessionStatus.Closed;
            session.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            string safeGroupId = session.Id.ToString().ToLower().Trim();
            await _hub.Clients.Group(safeGroupId).SendAsync("SessionClosed", new
            {
                chatSessionId = session.Id,
                message = "Phiên hỗ trợ đã kết thúc"
            });

            return true;
        }

        public async Task<ChatSession> ToggleSessionStatusAsync(Guid userId, string target)
        {
            // 1. Tìm thông tin khách hàng từ UserId
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId)
                ?? throw new KeyNotFoundException("Không tìm thấy khách hàng.");

            // 2. Tìm phiên chat đang hoạt động (chưa đóng) của khách
            var session = await _context.ChatSessions
                .FirstOrDefaultAsync(s => s.CustomerId == customer.Id && s.Status != ChatSessionStatus.Closed);

            if (session == null)
            {
                if (target.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    session = new ChatSession
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        Status = ChatSessionStatus.WaitingForStaff,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.ChatSessions.Add(session);
                    // Lưu tạm xuống DB để tí nữa SaveChangesAsync một thể ở cuối hàm
                }
                else if (target.Equals("AI", StringComparison.OrdinalIgnoreCase))
                {
                    // Tương tự, nếu chưa có phòng mà mồi trạng thái AI thì tự tạo phòng AI luôn
                    session = new ChatSession
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        Status = ChatSessionStatus.WithAI,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.ChatSessions.Add(session);
                }
                else
                {
                    throw new BadRequestException("Mục tiêu chuyển đổi (Target) không hợp lệ.");
                }
            }
            else
            {
                // 3. Nếu ĐÃ CÓ phiên chat đang chạy, tiến hành Đổi trạng thái cũ bình thường
                if (target.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    session.Status = ChatSessionStatus.WaitingForStaff; // Chuyển sang hàng đợi Nhân viên
                }
                else if (target.Equals("AI", StringComparison.OrdinalIgnoreCase))
                {
                    session.Status = ChatSessionStatus.WithAI; // Quay lại chế độ AI tự động trả lời
                }
                else
                {
                    throw new BadRequestException("Mục tiêu chuyển đổi (Target) không hợp lệ.");
                }

                session.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<List<ChatSessionResponse>> GetActiveSessionsByStaffAsync(Guid userId)
        {
            var staff = await _context.Staffs
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (staff == null) return null;

            var sessions = await _context.ChatSessions
                .Include(s => s.Customer)
                .Include(s => s.ChatMessages)
                .Include(s => s.Staff)
                .Where(s => s.StaffId == staff.Id && s.Status != ChatSessionStatus.Closed)
                .ToListAsync();

            return sessions.Select(session => new ChatSessionResponse
            {
                Id = session.Id,
                CustomerId = session.CustomerId,
                CustomerName = session.Customer?.FullName ?? "Khách hàng ẩn danh", // <--- Lấy tên KH, không phải tên Staff
                Status = session.Status.ToString(),
                StaffId = session.StaffId,
                StaffName = session.Staff?.FullName ?? "Nhân viên hỗ trợ",
                CreatedAt = session.CreatedAt,
                Messages = session.ChatMessages
             .OrderBy(m => m.CreatedAt)
             .Select(m => new ChatMessageResponse
             {
                 Id = m.Id,
                 SenderType = m.SenderType,
                 Message = m.Message,
                 // Nếu là User gửi -> Lấy tên Khách hàng. Nếu là Staff gửi -> Lấy tên Staff
                 SenderName = m.SenderType == "User"
                     ? (session.Customer?.FullName ?? "Khách hàng")
                     : (session.Staff?.FullName ?? "Nhân viên"),
                 CreatedAt = m.CreatedAt
             }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<ChatSessionResponse>> GetCustomerChatHistoryAsync(Guid userId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (customer == null) return Enumerable.Empty<ChatSessionResponse>();

            return await _context.ChatSessions
                .Include(s => s.Customer)
                .Include(s => s.Staff)
                .Include(s => s.ChatMessages)
                .Where(s => s.CustomerId == customer.Id)
                .OrderByDescending(s => s.CreatedAt) // Phiên gần nhất xếp lên đầu
                .Select(s => new ChatSessionResponse
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.FullName,
                    Status = s.Status.ToString(),
                    StaffId = s.StaffId,
                    StaffName = s.Staff != null ? s.Staff.FullName : "Hệ thống / AI",
                    CreatedAt = s.CreatedAt,
                    Messages = s.ChatMessages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new ChatMessageResponse
                        {
                            Id = m.Id,
                            SenderType = m.SenderType,
                            Message = m.Message,
                            SenderName = m.SenderType == "User"
                                ? s.Customer.FullName
                                : (s.Staff != null ? s.Staff.FullName : "AI Hỗ trợ"),
                            CreatedAt = m.CreatedAt
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<IEnumerable<ChatSessionResponse>> GetAllChatHistoryAsync()
        {
            return await _context.ChatSessions
                .Include(s => s.Customer)
                .Include(s => s.Staff)
                .Include(s => s.ChatMessages)
                .OrderByDescending(s => s.CreatedAt) // Phiên mới nhất lên đầu
                .Select(s => new ChatSessionResponse
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.FullName,
                    Status = s.Status.ToString(),
                    StaffId = s.StaffId,
                    StaffName = s.Staff != null ? s.Staff.FullName : "Hệ thống / AI",
                    CreatedAt = s.CreatedAt,
                    Messages = s.ChatMessages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new ChatMessageResponse
                        {
                            Id = m.Id,
                            SenderType = m.SenderType,
                            Message = m.Message,
                            SenderName = m.SenderType == "User"
                                ? s.Customer.FullName
                                : (s.Staff != null ? s.Staff.FullName : "AI Hỗ trợ"),
                            CreatedAt = m.CreatedAt
                        }).ToList()
                }).ToListAsync();
        }
    }
}
