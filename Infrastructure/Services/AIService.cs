using Application.DTOs.Feedback;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hub;
        public AIService(HttpClient httpClient, IConfiguration configuration, IApplicationDbContext context, IHubContext<ChatHub> hub)
        {
            _httpClient = httpClient;
            _context = context;
            _hub = hub;
            // KHÔNG ném lỗi ở constructor: AIService được DI vào ChatController, nên thiếu key
            // sẽ làm chết cả controller => toàn bộ API chat trả 500 dù chat người-với-người
            // không cần AI. Chỉ báo lỗi tại đúng chỗ thực sự gọi Gemini (CallGeminiAsync).
            _apiKey = configuration["GeminiSettings:ApiKey"] ?? string.Empty;
        }

        private async Task<string> CallGeminiAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new AppException("Tính năng trợ lý AI chưa được cấu hình (thiếu Gemini API Key).", 503);

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                // 1. Gửi request và nhận response thông thường
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                // 2. Nếu Google trả về mã lỗi (400, 401, 403, 429, 500...)
                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Thử parse cấu trúc lỗi chuẩn của Google để lấy thông báo chi tiết
                        using var errorDoc = JsonDocument.Parse(responseString);
                        if (errorDoc.RootElement.TryGetProperty("error", out var errorEl))
                        {
                            var code = errorEl.GetProperty("code").GetInt32();
                            var message = errorEl.GetProperty("message").GetString();
                            var status = errorEl.GetProperty("status").GetString();

                            throw new Exception($"Gemini API Error [{status} - {code}]: {message}");
                        }
                    }
                    catch (Exception ex) when (!(ex is Exception && ex.Message.StartsWith("Gemini API Error")))
                    {
                        // Nếu không parse được cấu trúc lỗi của Google, ném ra chuỗi thô ban đầu
                    }

                    throw new Exception($"Gemini HTTP Error: {(int)response.StatusCode} {response.ReasonPhrase} - Details: {responseString}");
                }

                // 3. Nếu Success nhưng Response trống hoặc null
                if (string.IsNullOrWhiteSpace(responseString))
                {
                    throw new Exception("Gemini Error: Nhận được phản hồi rỗng (Empty response) từ Google server.");
                }

                // 4. Parse dữ liệu khi thành công (Có bọc kiểm tra an toàn bằng TryGetProperty)
                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];

                    // Kiểm tra xem câu trả lời có bị chặn bởi bộ lọc an toàn (Safety Ratings) không
                    if (firstCandidate.TryGetProperty("finishReason", out var reason) && reason.GetString() != "STOP")
                    {
                        var reasonStr = reason.GetString();
                        if (reasonStr == "SAFETY" || reasonStr == "RECITATION")
                        {
                            throw new Exception($"Gemini Blocked: Nội dung bị Google chặn do vi phạm chính sách hoặc lý do an toàn ({reasonStr}).");
                        }
                    }

                    if (firstCandidate.TryGetProperty("content", out var resContent) &&
                        resContent.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        return parts[0].GetProperty("text").GetString() ?? string.Empty;
                    }
                }

                throw new Exception($"Gemini Parse Error: Cấu trúc JSON thay đổi hoặc không tìm thấy trường dữ liệu 'text'. Response thô: {responseString}");
            }
            catch (HttpRequestException netEx)
            {
                // 5. Bắt lỗi kết nối mạng (Timeout, rớt mạng, DNS không phân giải được, sai URL...)
                throw new Exception($"Gemini Network Connection Error: Không thể kết nối tới máy chủ Google Generative Language. Chi tiết: {netEx.Message}", netEx);
            }
            catch (Exception ex)
            {
                // Giữ nguyên các Exception có chủ đích được throw ở trên, tránh bọc lại vô nghĩa
                if (ex.Message.StartsWith("Gemini ")) throw;

                throw new Exception($"Gemini Unexpected Error: {ex.Message}", ex);
            }
        }

        public async Task<(string sessionId, string response)> ChatWithCustomerAsync(Guid userId, string customerMessage)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.UserId == userId)
                ?? throw new KeyNotFoundException("Không tìm thấy thông tin khách hàng tương ứng với tài khoản này.");
            var customerId = customer.Id;

            var session = await _context.ChatSessions
                .Include(s => s.ChatMessages)
                .FirstOrDefaultAsync(s => s.CustomerId == customerId && s.Status != ChatSessionStatus.Closed);

            if (session == null)
            {
                session = new ChatSession
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    Status = ChatSessionStatus.WithAI,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ChatSessions.Add(session);
                await _context.SaveChangesAsync();
            }

            var customerMsgEntity = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = session.Id,
                CustomerId = customerId,
                SenderType = "User",
                Message = customerMessage,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(customerMsgEntity);
            session.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await _hub.Clients.Group(session.Id.ToString()).SendAsync("ReceiveMessage", new
            {
                senderType = customerMsgEntity.SenderType,
                message = customerMsgEntity.Message,
                createdAt = customerMsgEntity.CreatedAt
            });
            var sessionId = session.Id.ToString();
            // CHẶN GỌI AI TẠI ĐÂY (Sau khi tin nhắn của khách đã được lưu vào DB an toàn)
            if (session.Status == ChatSessionStatus.WaitingForStaff || session.Status == ChatSessionStatus.HandledByStaff)
            {
                return (sessionId, "LIVE_CHAT_MODE");
            }

            var activePackages = await _context.WashPackages
                .Where(p => p.IsActive)
                .Select(p => $"- Gói '{p.Name}': Giá {p.Price:N0} VNĐ, thời gian {p.DurationMinutes} phút. Mô tả: {p.Description}")
                .ToListAsync();

            var activeBranches = await _context.Branches
                .Where(b => b.Status == BranchStatus.Active)
                .Select(b => $"- Chi nhánh '{b.BranchName}': Địa chỉ tại {b.Address}. Hotline: {b.Hotline}")
                .ToListAsync();

            var packagesContext = string.Join("\n", activePackages);
            var branchesContext = string.Join("\n", activeBranches);

            var historyList = session.ChatMessages
            .OrderByDescending(m => m.CreatedAt)
            .Take(6)
            .OrderBy(m => m.CreatedAt)
            .Select(m => $"{m.SenderType}: {m.Message}")
            .ToList();
            var chatHistoryContext = string.Join("\n", historyList);

            // 2. Thiết kế Prompt
            var systemPrompt = $@"
            Bạn là một trợ lý ảo thông minh, chuyên nghiệp của hệ thống Rửa Xe Tự Động.
            Nhiệm vụ của bạn là tư vấn đúng theo các thông tin dịch vụ và chi nhánh thực tế được cung cấp dưới đây. 
            Tuyệt đối không tự bịa ra thông tin dịch vụ hay giá cả không có trong danh sách.

            DANH SÁCH GÓI RỬA XE HIỆN CÓ CỦA HỆ THỐNG:
            {packagesContext}

            DANH SÁCH CHI NHÁNH ĐANG HOẠT ĐỘNG:
            {branchesContext}

            ---
            LƯU Ý KHI TRẢ LỜI:
            - Trả lời ngắn gọn, lịch sự, xưng hô ""Dạ"", ""Chào bạn"", ""Hệ thống bên em""...
            - Khi khách hỏi cách thức rửa xe hoặc chọn dịch vụ, hãy gợi ý cho họ các gói cụ thể ở trên kèm giá tiền để họ chọn.
            - Nếu khách hỏi cần tư vấn chuyên sâu, báo giá sửa chữa lớn, phàn nàn gay gắt hoặc muốn gặp nhân viên, hãy nói chính xác câu: ""Yêu cầu của bạn đã được chuyển đến nhân viên chi nhánh, vui lòng đợi trong giây lát.""

            Lịch sử cuộc trò chuyện (nếu có):
            {(chatHistoryContext == "string" ? "" : chatHistoryContext)}

            Tin nhắn mới của khách hàng: ""{customerMessage}""
            Trợ lý ảo phản hồi ngắn gọn:";

            string aiResponse;
            try
            {
                // Gọi API Gemini
                aiResponse = await CallGeminiAsync(systemPrompt);

                // Kiểm tra từ khóa không phân biệt hoa thường
                if (aiResponse.Contains("chuyển đến nhân viên chi nhánh", StringComparison.OrdinalIgnoreCase))
                {
                    session.Status = ChatSessionStatus.WaitingForStaff;
                }
            }
            catch (Exception ex)
            {
                // BẮT LỖI TẠI ĐÂY: Nếu Gemini lỗi, gán nội dung lỗi chi tiết vào aiResponse để hiển thị trực tiếp lên UI chat
                aiResponse = $"[HỆ THỐNG DEBUG ĐANG BẬT] Đã xảy ra lỗi khi gọi AI. Chi tiết lỗi: {ex.Message}";

                // Bạn có thể log ra console server để xem đầy đủ StackTrace (nếu cần)
                Console.WriteLine($"=== AI SERVICE ERROR ===\n{ex.ToString()}\n========================");
            }

            // 3. Lưu phản hồi của AI (hoặc nội dung lỗi) vào Database để cuộc trò chuyện không bị gián đoạn
            var aiMsgEntity = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = session.Id,
                CustomerId = customerId,
                SenderType = "AI",
                Message = aiResponse,
                CreatedAt = DateTime.UtcNow
            };
            _context.ChatMessages.Add(aiMsgEntity);
            session.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Gửi qua SignalR về cho Client hiển thị lên khung chat
            await _hub.Clients.Group(session.Id.ToString()).SendAsync("ReceiveMessage", new
            {
                senderType = aiMsgEntity.SenderType,
                message = aiMsgEntity.Message,
                createdAt = aiMsgEntity.CreatedAt
            });

            return (sessionId, aiResponse);
        }

        public async Task<FeedbackModerationResponse> ModerateFeedbackAsync(string feedbackComment)
        {
            var prompt = $@"
            Bạn là một hệ thống kiểm duyệt nội dung tự động cho nền tảng đặt lịch rửa xe.
            Nhiệm vụ của bạn là kiểm tra xem bình luận của khách hàng có hợp lệ hay không.
            Bình luận KHÔNG HỢP LỆ bao gồm: chứa từ ngữ tục tĩu, chửi bới, xúc phạm, phân biệt chủng tộc, spam quảng cáo, nội dung nhạy cảm, phá hoại hệ thống bằng các ký tự lạ hoặc hoàn toàn không liên quan tới dịch vụ/nhân viên tiệm rửa xe.

            Bình luận cần kiểm tra: ""{feedbackComment}""

            Hãy trả về một chuỗi JSON duy nhất, không kèm theo bất kỳ văn bản giải thích nào khác ngoài JSON, định dạng như sau:
            {{
                ""IsValid"": true hoặc false,
                ""Reason"": ""Nếu IsValid là false, hãy nêu rõ lý do tại đây bằng 1 câu ngắn gọn. Nếu true thì để trống"",
                ""CleanedComment"": ""Nếu bình luận hợp lệ nhưng có chứa một vài từ nói giảm nói tránh nhẹ, hãy sửa lại cho lịch sự, nếu không thì giữ nguyên""
            }}";

            string rawJson = await CallGeminiAsync(prompt);

            // Xử lý loại bỏ markdown ```json ... ``` nếu Gemini tự động bọc lại
            if (rawJson.Contains("```json"))
            {
                rawJson = rawJson.Replace("```json", "").Replace("```", "").Trim();
            }

            try
            {
                var result = JsonSerializer.Deserialize<FeedbackModerationResponse>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result ?? new FeedbackModerationResponse { IsValid = false, Reason = "Không thể phân tích dữ liệu AI." };
            }
            catch
            {
                // Dự phòng trường hợp AI không trả về đúng định dạng JSON mong muốn
                return new FeedbackModerationResponse { IsValid = true, Reason = "Lỗi parse dữ liệu AI, tạm thời duyệt." };
            }
        }
    }
}
