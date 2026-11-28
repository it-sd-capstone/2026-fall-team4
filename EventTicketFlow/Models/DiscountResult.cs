namespace EventTicketFlow.Models
{
    // Checks if the discount code is valid
    public class DiscountResult
    {
        public bool IsValid { get; set; }
        public int PercentOff { get; set; }
        public string Message { get; set; } = "";
    }
}