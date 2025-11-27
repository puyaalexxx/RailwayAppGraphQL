using MassTransit;
using RailwayAppGraphQL.Events.Tickets;

namespace RailwayAppGraphQL.Consumers.Tickets;

public class TicketCreatedConsumer : IConsumer<TicketCreated>
{
    public Task Consume(ConsumeContext<TicketCreated> context)
    {
        var message = context.Message;

        Console.WriteLine($"Ticket created: {message.TicketId}");

        /*Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Postcard);
                    page.Margin(20);

                    page.Header().Text("Your Train Ticket").Bold().FontSize(18);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Ticket #: {Ticket.Number}");
                        col.Item().Text($"Passenger: {Ticket.PassengerName}");
                        col.Item().Text($"Train: {Ticket.TrainNumber}");
                        col.Item().Text($"Seat: {Ticket.SeatNumber}");
                        col.Item().Text($"Departure (UTC): {Ticket.DepartureTimeUtc}");
                        col.Item().Text($"Arrival (UTC): {Ticket.ArrivalTimeUtc}");
                        // add more styling: lines, tables, QR code/images etc.
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated: {DateTime.UtcNow:yyyy‑MM‑dd HH:mm} UTC");
                });
            })
            .GeneratePdf($"Pdf/Tickets/tickets/ticket_{ticket.Id}.pdf");*/

        return Task.CompletedTask;
    }
}