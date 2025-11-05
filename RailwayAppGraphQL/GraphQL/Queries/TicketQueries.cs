using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class TicketQueries
{
    public IQueryable<Ticket> GetTickets(ApplicationDbContext dbContext)
    {
        return dbContext.Tickets;
    }
}