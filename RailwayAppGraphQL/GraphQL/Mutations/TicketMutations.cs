using FluentValidation;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;
using RailwayAppGraphQL.Models.Tickets;

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

    public async Task<Ticket> CreateTicket(ApplicationDbContext dbContext, CreateTicketInput input)
    {
        var validationResult = await _createValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Number = input.Number,
            PassengerName = input.PassengerName,
            PassengerEmail = input.PassengerEmail,
            SeatNumber = input.SeatNumber,
            Price = input.Price,
            Currency = input.Currency,
            PurchasedAtUtc = input.PurchasedAtUtc,
            TrainId = input.TrainId
        };

        dbContext.Tickets.Add(ticket);

        await dbContext.SaveChangesAsync();

        return ticket;
    }
    
}