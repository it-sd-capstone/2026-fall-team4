namespace EventTicketFlow.Models
{
    public class HomeViewModel
    {
        public List<Event> Events { get; set; } = new();

        public string? SearchTerm { get; set; }
    }
}
