namespace RailwayAppGraphQL.Events.Tickets;

public sealed record TicketUpdated(
    Guid TicketId,
    string Number,
    string PassengerName,
    string PassengerEmail,
    string SeatNumber,
    decimal Price,
    string Currency,
    DateTime PurchasedAtUtc,
    string TrainNumber,
    string TrainName,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    string DepartureStation,
    string ArrivalStation
);