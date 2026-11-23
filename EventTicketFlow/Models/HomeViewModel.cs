namespace EventTicketFlow.Models
{
    public class HomeViewModel
    {
        public List<EventCardViewModel> Events { get; set; } = new();

        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Price { get; set; }
        public string? Sort { get; set; }
        public EventCardViewModel? FeaturedEvent { get; set; }

        public bool HasFilters => !string.IsNullOrWhiteSpace(SearchTerm) || Status != null || Price != null || Sort != null;
    }
}
