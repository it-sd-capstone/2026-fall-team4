using EventTicketFlow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventTicketFlow.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Using restrict so that an event, ticket type, or customer that already
            // has sales can't be deleted, so purchase history is never wiped by accident.
            modelBuilder.Entity<TicketType>()
                .HasOne(t => t.Event).WithMany()
                .HasForeignKey(t => t.EventId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasedTicket>()
                .HasOne(p => p.TicketType).WithMany()
                .HasForeignKey(p => p.TicketTypeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchasedTicket>()
                .HasOne(p => p.Customer).WithMany(c => c.Tickets)
                .HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<Settings> Settings { get; set; }

    }
}