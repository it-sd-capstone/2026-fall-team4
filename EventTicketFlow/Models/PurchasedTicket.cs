using System.ComponentModel.DataAnnotations;

namespace EventTicketFlow.Models
{
    public class PurchasedTicket
    {
        [Key]
        public int Id { get; set; }
        public Guid CheckinUUID{ get; set; }
        public TicketType? TicketType { get; set; }
        public Customer? Customer { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "VIP")]
        public bool IsVip { get; set; }
        [Display(Name = "Fast Track")]
        public bool IsFastTrack { get; set; }
        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
        [Display(Name = "Date Purchased")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Check In Date")]
        public DateTime? CheckInDate { get; set; }

    }
}
