using Application.DTOs.Feedback;

namespace Application.DTOs.ChatFeedback
{
    public class ChatStaffStatisticResponse
    {
        public List<StaffRatingResponse> TopChatStaffs { get; set; } = new();
        public List<StaffRatingResponse> LowestChatStaffs { get; set; } = new();
    }
}
