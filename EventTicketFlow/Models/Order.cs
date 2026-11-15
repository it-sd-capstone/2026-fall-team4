namespace EventTicketFlow.Models
{
    public class Order
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }
        public decimal total { get; set; }
        public List<PurchasedTicket> Tickets { get; set; } = new();
    }
}
