using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventTicketFlow.Models

{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public Guid CheckinUuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address {  get; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<Order> Orders { get; set; } = new(); 

        public List<PurchasedTicket> Tickets { get; set; } = new();
    }
}
