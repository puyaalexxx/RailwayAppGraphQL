using FluentValidation;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs.Stops;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class StopMutations
{
    private readonly IValidator<CreateStopInput> _createValidator;
    private readonly IValidator<UpdateStopInput> _updateValidator;

    public StopMutations(IValidator<CreateStopInput> createValidator, IValidator<UpdateStopInput> updateValidator)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Stop> CreateStop(ApplicationDbContext dbContext, CreateStopInput input)
    {
        var validationResult = await _createValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var stop = new Stop
        {
            Id = Guid.NewGuid(),
            StationId = input.StationId,
            TrainId = input.TrainId,
            DepartureTimeUtc = input.DepartureTimeUtc,
            ArrivalTimeUtc = input.ArrivalTimeUtc
        };

        dbContext.Stops.Add(stop);

        await dbContext.SaveChangesAsync();

        return stop;
    }
}