using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Stops;

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
}