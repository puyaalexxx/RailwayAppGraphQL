using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.GraphQL.Mutations;
using RailwayAppGraphQL.GraphQL.Queries;

namespace RailwayAppGraphQL.Extensions;

public static class GraphQlExtensions
{
    public static void AddGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .RegisterDbContextFactory<ApplicationDbContext>()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddTypes(typeof(TrainQueries), typeof(StationQueries), typeof(TicketQueries), typeof(StopQueries),
                typeof(TrainMutations), typeof(StationMutations), typeof(TicketMutations), typeof(StopMutations))
            .ModifyPagingOptions(pagingOptions =>
            {
                pagingOptions.DefaultPageSize = 5;
                pagingOptions.MaxPageSize = 10;
                pagingOptions.AllowBackwardPagination = false;
                // pagingOptions.RequirePagingBoundaries = true; // clients need to specify either first, last or take.
            })
            .AddProjections(); // select only required fields not all of them
        //.AddFiltering()
        //.AddSorting();
    }
}