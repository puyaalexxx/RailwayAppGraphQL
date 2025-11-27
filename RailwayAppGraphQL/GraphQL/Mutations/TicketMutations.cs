using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class TicketMutations
{
    private readonly IValidator<CreateTicketInput> _createValidator;
    private readonly IValidator<UpdateTicketInput> _updateValidator;

    public TicketMutations(IValidator<CreateTicketInput> createValidator, IValidator<UpdateTicketInput> updateValidator)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }
}