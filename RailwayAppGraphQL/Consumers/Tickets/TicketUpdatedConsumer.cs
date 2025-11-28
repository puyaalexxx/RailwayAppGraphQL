using MassTransit;
using RailwayAppGraphQL.Events.Tickets;

namespace RailwayAppGraphQL.Consumers.Tickets;

public class TicketUpdatedConsumer : IConsumer<TicketUpdated>
{
    public Task Consume(ConsumeContext<TicketUpdated> context)
    {
        var ticket = context.Message;

        Console.WriteLine($"Ticket updated: {ticket.TicketId}");

        return Task.CompletedTask;
    }
}