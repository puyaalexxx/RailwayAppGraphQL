using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class StopQueries
{
    [UseProjection]
    public IQueryable<Stop> GetStops(ApplicationDbContext dbContext)
    {
        return dbContext.Stops.AsNoTracking();
    }

    [UseProjection]
    public IQueryable<Stop> GetStopById(ApplicationDbContext dbContext, Guid stopId)
    {
        return dbContext.Stops
            .AsNoTracking()
            .Where(s => s.Id == stopId);
    }
}