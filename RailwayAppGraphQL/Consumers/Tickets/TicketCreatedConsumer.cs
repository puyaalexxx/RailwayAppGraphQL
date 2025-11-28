using MassTransit;
using QuestPDF.Fluent;
using RailwayAppGraphQL.Events.Tickets;
using RailwayAppGraphQL.Helpers.Mappings;
using RailwayAppGraphQL.Pdf;

namespace RailwayAppGraphQL.Consumers.Tickets;

public class TicketCreatedConsumer : IConsumer<TicketCreated>
{
    public Task Consume(ConsumeContext<TicketCreated> context)
    {
        var ticket = context.Message;

        Console.WriteLine($"Ticket created: {ticket.TicketId}");

        // generate PDF with ticket info
        var document = Document.Create(container => new TrainTicketDocument(ticket.ToDto()).Compose(container));
        document.GeneratePdf($"Pdf/Tickets/ticket_{ticket.TicketId}.pdf");

        return Task.CompletedTask;
    }
}