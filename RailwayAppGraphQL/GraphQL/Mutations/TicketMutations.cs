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

    public async Task<Ticket> UpdateTicket(ApplicationDbContext dbContext, Guid ticketId, UpdateTicketInput input)
    {
        var validationResult = await _updateValidator.ValidateAsync(input);
        if (!validationResult.IsValid) throw new GraphQLException(validationResult.ToGraphQLErrors());

        var ticket = await dbContext.Tickets.FindAsync(ticketId);
        if (ticket == null) throw new GraphQLException("Ticket not found.");

        // Update only the provided fields
        if (input.Number != null) ticket.Number = input.Number;
        if (input.PassengerName != null) ticket.PassengerName = input.PassengerName;
        if (input.PassengerEmail != null) ticket.PassengerEmail = input.PassengerEmail;
        if (input.SeatNumber != null) ticket.SeatNumber = input.SeatNumber;
        if (input.Price != null) ticket.Price = input.Price.Value;
        if (input.Currency != null) ticket.Currency = input.Currency.Value;
        if (input.PurchasedAtUtc != null) ticket.PurchasedAtUtc = input.PurchasedAtUtc.Value; 
        if (input.TrainId != null) ticket.TrainId = input.TrainId.Value;

        await dbContext.SaveChangesAsync();

        return ticket;
    }
    
    public async Task<Ticket> DeleteTicket(ApplicationDbContext dbContext, Guid ticketId)
    {
        var ticket = await dbContext.Tickets.FindAsync(ticketId);
        if (ticket == null) throw new GraphQLException("Ticket not found.");

        dbContext.Tickets.Remove(ticket);

        await dbContext.SaveChangesAsync();

        return ticket;
    }
}