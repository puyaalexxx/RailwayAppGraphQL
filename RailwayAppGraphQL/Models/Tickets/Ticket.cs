using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Models.Tickets;

public sealed class Ticket
{
    public Guid Id { get; set; }
    
    public required string Number { get; set; }
    
    public string? PassengerName { get; set; }
    public string? PassengerEmail { get; set; }
    
    public string? SeatNumber { get; set; }

    public decimal Price { get; set; }
    public Currency Currency { get; set; } = Currency.USD;
    
    public DateTime PurchasedAtUtc { get; set; } = DateTime.UtcNow;
    
    // Foreign key to Train
    public Guid TrainId { get; set; }
    // Navigation property
    public Train Train { get; set; } = null!;
}