using System;

namespace Domain.Entities
{
    public class EmailVerification
    {
        public Guid Id { get; set; }

        // Liên kết trực tiếp tới tài khoản User cần xác thực
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Email nhận mã xác thực (phòng trường hợp User đổi email mới nhưng chưa verify)
        public string Email { get; set; } = null!;

        // Mã xác thực (Có thể là chuỗi Token dài hoặc mã OTP 6 số)
        public string VerificationToken { get; set; } = null!;

        // Thời gian mã này hết hiệu lực (thường là 10 - 15 phút cho OTP hoặc 24 giờ cho Link Token)
        public DateTime ExpiresAt { get; set; }

        // Đánh dấu mã này đã được sử dụng hay chưa
        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? VerifiedAt { get; set; }
    }
}