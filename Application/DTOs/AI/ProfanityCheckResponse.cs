namespace Application.DTOs.AI
{
    public class ProfanityCheckResponse
    {
        public bool IsProfane { get; set; }
        public string Reason { get; set; }
        public string Matched { get; set; }
    }
}
