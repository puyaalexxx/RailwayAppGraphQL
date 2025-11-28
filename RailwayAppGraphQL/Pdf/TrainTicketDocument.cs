using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RailwayAppGraphQL.Events.Tickets;

namespace RailwayAppGraphQL.Pdf;

public class TrainTicketDocument : IDocument
{
    private readonly TicketCreated _ticket;

    public TrainTicketDocument(TicketCreated ticket)
    {
        _ticket = ticket;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(new PageSize(227, 142)); // 8x5 cm ticket
            page.Margin(10); // Page margin only

            page.Content().Column(stack =>
            {
                stack.Item().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.White).Padding(10)
                    .Column(innerStack =>
                    {
                        // Header
                        innerStack.Item().Row(row =>
                        {
                            row.RelativeItem().Text("TRAIN TICKET").Bold().FontSize(14).FontColor(Colors.Blue.Darken2);

                            row.ConstantItem(80).AlignRight().Column(column =>
                            {
                                column.Item().AlignRight().Text($"Train No: {_ticket.TrainNumber}").FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);

                                column.Item().AlignRight().Text($"Ticket No: {_ticket.Number}").FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });

                        innerStack.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        innerStack.Item().PaddingTop(5).AlignCenter().Text("Train: " + _ticket.TrainName).Bold()
                            .FontSize(8);

                        // Details
                        innerStack.Item().PaddingVertical(5).Column(details =>
                        {
                            details.Item().Row(row =>
                            {
                                row.RelativeItem().Text(text =>
                                {
                                    text.Span("From: ").Bold().FontSize(6);
                                    text.Span(_ticket.DepartureStation).FontSize(6);
                                });
                                row.ConstantItem(80).AlignRight().Text(text =>
                                {
                                    text.Span("To: ").Bold().FontSize(6);
                                    text.Span(_ticket.ArrivalStation).FontSize(6);
                                });
                            });

                            //departure
                            details.Item().Row(row =>
                            {
                                row.RelativeItem().Text(text =>
                                {
                                    text.Span("Departure: ").Bold().FontSize(6);
                                    text.Span(_ticket.DepartureTime.ToString("yyyy-MM-dd HH:mm")).FontSize(6);
                                });
                                row.ConstantItem(80).AlignRight().Text(text =>
                                {
                                    text.Span("Arrival: ").Bold().FontSize(6);
                                    text.Span(_ticket.ArrivalTime.ToString("yyyy-MM-dd HH:mm")).FontSize(6);
                                });
                            });

                            // Passenger info
                            details.Item().Row(row =>
                            {
                                row.RelativeItem().Text(text =>
                                {
                                    text.Span("Passenger: ").Bold().FontSize(6);
                                    text.Span(_ticket.PassengerName).FontSize(6);
                                });
                                row.ConstantItem(80).AlignRight().Text(text =>
                                {
                                    text.Span("Email: ").Bold().FontSize(6);
                                    text.Span(_ticket.PassengerEmail).FontSize(6);
                                });
                            });

                            details.Item().Row(row =>
                            {
                                row.RelativeItem().Text(text =>
                                {
                                    text.Span("Seat: ").Bold().FontSize(6);
                                    text.Span(_ticket.SeatNumber).FontSize(6);
                                });
                                row.ConstantItem(80).AlignRight().Text(text =>
                                {
                                    text.Span("Price: ").Bold().FontSize(6);
                                    text.Span($"{_ticket.Price} {_ticket.Currency}").FontSize(6);
                                });
                            });

                            details.Item().PaddingBottom(2).PaddingTop(5).LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten1);

                            // Purchase date
                            details.Item().Text("Purchased at: " + _ticket.PurchasedAtUtc.ToString("d")).Bold()
                                .FontSize(8);
                        });
                    });
            });
        });
    }
}