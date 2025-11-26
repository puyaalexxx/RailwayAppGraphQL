using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class StationQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    public IQueryable<Station> GetStations(ApplicationDbContext dbContext)
    {
        return dbContext.Stations.AsNoTracking();
    }

    [UseProjection]
    public IQueryable<Station> GetStationById(ApplicationDbContext dbContext, Guid stationId)
    {
        return dbContext.Stations
            .AsNoTracking()
            .Where(s => s.Id == stationId);
    }
}