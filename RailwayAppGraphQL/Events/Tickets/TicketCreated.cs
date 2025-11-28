using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.Events.Tickets;

public sealed record TicketCreated(
    Guid TicketId,
    string Number,
    string PassengerName,
    string PassengerEmail,
    string SeatNumber,
    decimal Price,
    Currency Currency,
    DateTime PurchasedAtUtc,
    string TrainNumber,
    string TrainName,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    string DepartureStation,
    string ArrivalStation);