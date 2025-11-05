using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class StationQueries
{
    public IQueryable<Station> GetStations(ApplicationDbContext dbContext)
    {
        return dbContext.Stations;
    }
}