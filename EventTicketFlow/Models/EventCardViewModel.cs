namespace EventTicketFlow.Models
{
    // For the event card display on the home page
    public class EventCardViewModel
    {
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal? GeneralPrice { get; set; }

        // These are for the text in the cards
        public string Status
        {
            get
            {
                DateTime now = DateTime.Now;

                if (OpenDate > now)
                {
                    return "upcoming";
                }
                if (CloseDate < now)
                {
                    return "closed";
                }
                return "open"; 
            }

        }

        public string StatusText
        {
            get
            {
                if (Status == "upcoming")
                {
                    return "Upcoming";
                }
                if (Status == "closed")
                {
                    return "Closed";
                }
                return "Available";
            }
        }

        public string DateText
        {
            get
            {
                if (Status == "upcoming")
                {
                    return "Tickets on sale " + OpenDate.ToString("MMM d, yyyy ");
                }
                if (Status == "closed")
                {
                    return "Event ended on " + CloseDate.ToString("MMM d, yyyy ");
                }
                return "Sale ends on " + CloseDate.ToString("MMM d, yyyy ");
            }
        }

        public string PriceText
        {
            get
            {
                if (GeneralPrice.HasValue)
                {
                    return "From $" + GeneralPrice.Value.ToString("0.00");
                }
                return "No Price Information";
            }
        }
    }
     
}
