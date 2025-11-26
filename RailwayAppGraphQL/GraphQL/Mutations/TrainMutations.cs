using FluentValidation;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class TrainMutations
{
    private readonly IValidator<CreateTrainInput> _validator;

    public TrainMutations(IValidator<CreateTrainInput> validator)
    {
        _validator = validator;
    }

    public async Task<Train> CreateTrain(
        ApplicationDbContext dbContext,
        CreateTrainInput input)
    {
        var validationResult = await _validator.ValidateAsync(input);
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
}