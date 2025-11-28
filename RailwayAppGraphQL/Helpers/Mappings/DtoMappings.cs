using RailwayAppGraphQL.DTOs;
using RailwayAppGraphQL.Events.Tickets;

namespace RailwayAppGraphQL.Helpers.Mappings;

public static class DtoMappings
{
    public static TicketPdfModelDto ToDto(this TicketCreated ticket)
    {
        return new TicketPdfModelDto(
            ticket.TicketId,
            ticket.Number,
            ticket.PassengerName,
            ticket.PassengerEmail,
            ticket.SeatNumber,
            ticket.Price,
            ticket.Currency,
            ticket.PurchasedAtUtc,
            ticket.TrainNumber,
            ticket.TrainName,
            ticket.DepartureTime,
            ticket.ArrivalTime,
            ticket.DepartureStation,
            ticket.ArrivalStation
        );
    }

    public static TicketPdfModelDto ToDto(this TicketUpdated ticket)
    {
        return new TicketPdfModelDto(
            ticket.TicketId,
            ticket.Number,
            ticket.PassengerName,
            ticket.PassengerEmail,
            ticket.SeatNumber,
            ticket.Price,
            ticket.Currency,
            ticket.PurchasedAtUtc,
            ticket.TrainNumber,
            ticket.TrainName,
            ticket.DepartureTime,
            ticket.ArrivalTime,
            ticket.DepartureStation,
            ticket.ArrivalStation
        );
    }
}