using MassTransit;
using RailwayAppGraphQL.Consumers.Tickets;
using RailwayAppGraphQL.Events.Tickets;

namespace RailwayAppGraphQL.Extensions;

public static class MassTransitExtensions
{
    public static void AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TicketCreatedConsumer>();
            x.AddConsumer<TicketDeletedConsumer>();
            x.AddConsumer<TicketUpdatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("ticket-created-queue",
                    e => { e.ConfigureConsumer<TicketCreatedConsumer>(context); });
                cfg.ReceiveEndpoint("ticket-deleted-queue",
                    e => { e.ConfigureConsumer<TicketDeletedConsumer>(context); });
                cfg.ReceiveEndpoint("ticket-updated-queue",
                    e => { e.ConfigureConsumer<TicketUpdatedConsumer>(context); });
            });
        });
    }
}