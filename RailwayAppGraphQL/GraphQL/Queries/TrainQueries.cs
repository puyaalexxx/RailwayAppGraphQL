using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class TrainQueries
{
    [UseProjection]
    public IQueryable<Train> GetTrains(ApplicationDbContext dbContext)
    {
        return dbContext.Trains.AsNoTracking();
    }

    [UseProjection]
    public IQueryable<Train> GetTrainById(ApplicationDbContext dbContext, Guid trainId)
    {
        return dbContext.Trains
            .AsNoTracking()
            .Where(t => t.Id == trainId);
    }
}