using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventCreated { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int MaxTickets { get; set; }

    }
}