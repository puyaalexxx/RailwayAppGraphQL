using MassTransit;
using QuestPDF.Fluent;
using RailwayAppGraphQL.Events.Tickets;
using RailwayAppGraphQL.Helpers.Mappings;
using RailwayAppGraphQL.Pdf;

namespace RailwayAppGraphQL.Consumers.Tickets;

public class TicketUpdatedConsumer : IConsumer<TicketUpdated>
{
    public Task Consume(ConsumeContext<TicketUpdated> context)
    {
        var ticket = context.Message;

        Console.WriteLine($"Ticket updated: {ticket.TicketId}");

        // regenerate PDF with ticket info
        var document = Document.Create(container => new TrainTicketDocument(ticket.ToDto()).Compose(container));
        document.GeneratePdf($"Pdf/Tickets/ticket_{ticket.TicketId}.pdf");

        return Task.CompletedTask;
    }
}