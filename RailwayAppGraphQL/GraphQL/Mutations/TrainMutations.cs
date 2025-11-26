using FluentValidation;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs.Trains;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class TrainMutations
{
    private readonly IValidator<CreateTrainInput> _createValidator;
    private readonly IValidator<UpdateTrainInput> _updateValidator;

    public TrainMutations(IValidator<CreateTrainInput> createValidator, IValidator<UpdateTrainInput> updateValidator)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Train> CreateTrain(ApplicationDbContext dbContext, CreateTrainInput input)
    {
        var validationResult = await _createValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var train = new Train
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Number = input.Number,
            Type = input.Type,
            Seats = input.Seats,
            Status = input.Status
        };

        dbContext.Trains.Add(train);

        await dbContext.SaveChangesAsync();

        return train;
    }

    public async Task<Train> UpdateTrain(ApplicationDbContext dbContext, Guid trainId, UpdateTrainInput input)
    {
        var validationResult = await _updateValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var train = await dbContext.Trains.FindAsync(trainId);
        if (train == null) throw new GraphQLException("Train not found");

        // Update only the provided fields
        if (input.Name != null) train.Name = input.Name;
        if (input.Number != null) train.Number = input.Number;
        if (input.Type.HasValue) train.Type = input.Type.Value;
        if (input.Seats.HasValue) train.Seats = input.Seats.Value;
        if (input.Status.HasValue) train.Status = input.Status.Value;

        await dbContext.SaveChangesAsync();

        return train;
    }

    public async Task<Train> DeleteTrain(ApplicationDbContext dbContext, Guid trainId)
    {
        var train = await dbContext.Trains.FindAsync(trainId);
        if (train == null) throw new GraphQLException("Train not found.");

        dbContext.Trains.Remove(train);

        await dbContext.SaveChangesAsync();

        return train;
    }
}