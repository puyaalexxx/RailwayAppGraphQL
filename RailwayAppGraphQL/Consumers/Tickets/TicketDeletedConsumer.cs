using MassTransit;
using RailwayAppGraphQL.Events.Tickets;
using Path = System.IO.Path;

namespace RailwayAppGraphQL.Consumers.Tickets;

public class TicketDeletedConsumer : IConsumer<TicketDeleted>
{
    public Task Consume(ConsumeContext<TicketDeleted> context)
    {
        var ticket = context.Message;

        Console.WriteLine($"Ticket deleted: {ticket.TicketId}");

        // Build the PDF file path
        var pdfPath = Path.Combine("Pdf", "Tickets", $"ticket_{ticket.TicketId}.pdf");

        // Check if the file exists and delete it
        if (File.Exists(pdfPath))
        {
            File.Delete(pdfPath);
            Console.WriteLine($"Deleted PDF for ticket {ticket.TicketId}");
        }
        else
        {
            Console.WriteLine($"PDF for ticket {ticket.TicketId} not found");
        }


        return Task.CompletedTask;
    }
}