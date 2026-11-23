using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Required, RegularExpression(@"^[\s\S]{1,100}$", ErrorMessage = "Event Name must be 100 characters or fewer.")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }
        [Required]
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }
        [Display(Name = "Event Created")]
        public DateTime EventCreated { get; set; }
        [Display(Name = "Open Date")]
        public DateTime OpenDate { get; set; }
        [Display(Name = "Close Date")]
        public DateTime CloseDate { get; set; }
        [Range(1, int.MaxValue)]
        [Display(Name = "Max Tickets")]
        public int MaxTickets { get; set; }

    }
}