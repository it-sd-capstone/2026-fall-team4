using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class TicketType
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        [Display(Name = "General Price")]
        public decimal GeneralPrice { get; set; }
        [Display(Name = "VIP Price")]
        public decimal VipPrice { get; set; }
        [Display(Name = "Fast Track Price")]
        public decimal FastTrackPrice { get; set; }

    }
}