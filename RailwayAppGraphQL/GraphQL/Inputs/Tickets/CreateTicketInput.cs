using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.GraphQL.Inputs.Tickets;

public sealed record CreateTicketInput(
    string Number,
    string PassengerName,
    string? PassengerEmail,
    string SeatNumber,
    decimal Price,
    Currency Currency,
    DateTime PurchasedAtUtc,
    Guid TrainId);