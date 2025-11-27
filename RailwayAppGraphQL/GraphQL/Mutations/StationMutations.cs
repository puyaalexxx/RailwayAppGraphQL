using FluentValidation;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs.Stations;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class StationMutations
{
    private readonly IValidator<CreateStationInput> _createValidator;
    private readonly IValidator<UpdateStationInput> _updateValidator;

    public StationMutations(
        IValidator<CreateStationInput> createValidator,
        IValidator<UpdateStationInput> updateValidator)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Station> CreateStation(ApplicationDbContext dbContext, CreateStationInput input)
    {
        var validationResult = await _createValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var station = new Station
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Address = input.Address,
            HasCoffeeMachine = input.HasCoffeeMachine,
            HasWaitingRoom = input.HasWaitingRoom,
            HasWc = input.HasWc
        };

        dbContext.Stations.Add(station);

        await dbContext.SaveChangesAsync();

        return station;
    }

    public async Task<Station> UpdateStation(ApplicationDbContext dbContext, Guid stationId, UpdateStationInput input)
    {
        var validationResult = await _updateValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var station = await dbContext.Stations.FindAsync(stationId);
        if (station == null) throw new GraphQLException("Station not found");

        // Update only the provided fields
        if (input.Name != null) station.Name = input.Name;
        if (input.Address != null) station.Address = input.Address;
        if (input.HasCoffeeMachine != null) station.HasCoffeeMachine = input.HasCoffeeMachine.Value;
        if (input.HasWaitingRoom != null) station.HasWaitingRoom = input.HasWaitingRoom.Value;
        if (input.HasWc != null) station.HasWc = input.HasWc.Value;

        await dbContext.SaveChangesAsync();

        return station;
    }

    public async Task<Station> DeleteStation(ApplicationDbContext dbContext, Guid stationId)
    {
        var station = await dbContext.Stations.FindAsync(stationId);
        if (station == null) throw new GraphQLException("Station not found.");

        dbContext.Stations.Remove(station);

        await dbContext.SaveChangesAsync();

        return station;
    }
}