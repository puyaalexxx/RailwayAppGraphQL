using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Events.Tickets;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;
using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class TicketMutations
{
    private readonly IBus _bus;
    private readonly IValidator<CreateTicketInput> _createValidator;
    private readonly IValidator<UpdateTicketInput> _updateValidator;

    public TicketMutations(IValidator<CreateTicketInput> createValidator, IValidator<UpdateTicketInput> updateValidator,
        IBus bus)
    {
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _bus = bus;
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

        // Load train with stops
        var train = await dbContext.Trains
            .Include(t => t.Stops.OrderBy(s => s.ArrivalTimeUtc))
            .ThenInclude(s => s.Station) // <-- include station here
            .FirstOrDefaultAsync(t => t.Id == ticket.TrainId);

        DateTime departureTime;
        DateTime arrivalTime;
        string departureStation;
        string arrivalStation;

        // we need to get the departure and arrival station names if train has only one stop or more than one stop
        // train exists because we checked it in the CreateTicketInputValidator
        if (train!.Stops.Count == 1)
        {
            var stop = train.Stops.First();
            departureTime = stop.DepartureTimeUtc;
            arrivalTime = stop.ArrivalTimeUtc;
            departureStation = stop.Station.Name;
            arrivalStation = stop.Station.Name;
        }
        else
        {
            var firstStop = train.Stops.First();
            var lastStop = train.Stops.Last();

            departureTime = firstStop.DepartureTimeUtc;
            arrivalTime = lastStop.ArrivalTimeUtc;
            departureStation = firstStop.Station.Name;
            arrivalStation = lastStop.Station.Name;
        }

        // Publish event
        await _bus.Publish(new TicketCreated(
            ticket.Id,
            ticket.Number,
            ticket.PassengerName,
            ticket.PassengerEmail ?? "",
            ticket.SeatNumber,
            ticket.Price,
            ticket.Currency,
            ticket.PurchasedAtUtc,
            //additional info
            train.Number,
            train.Name,
            departureTime,
            arrivalTime,
            departureStation,
            arrivalStation
        ));

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