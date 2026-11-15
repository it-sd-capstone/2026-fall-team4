using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class EventTicketFlowContext(DbContextOptions<EventTicketFlowContext> options) : IdentityDbContext<EventTicketFlow.Data.ApplicationUser>(options)
{
}
