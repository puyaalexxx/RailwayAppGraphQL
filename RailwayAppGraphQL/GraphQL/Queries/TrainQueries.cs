using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class TrainQueries
{
    [UseProjection]
    public IQueryable<Train> GetTrains(ApplicationDbContext dbContext)
    {
        return dbContext.Trains;
    }
}