namespace Application.DTOs.Auth
{
    public class EmailVerificationRequest
    {
        public Guid Id { get; set; }
        public string OtpCode { get; set; } = string.Empty;
    }
}
