using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class TicketQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<Ticket> GetTickets(ApplicationDbContext dbContext)
    {
        return dbContext.Tickets.AsNoTracking();
    }

    [UseProjection]
    public IQueryable<Ticket> GetTicketById(ApplicationDbContext dbContext, Guid ticketId)
    {
        return dbContext.Tickets
            .AsNoTracking()
            .Where(t => t.Id == ticketId);
    }
}