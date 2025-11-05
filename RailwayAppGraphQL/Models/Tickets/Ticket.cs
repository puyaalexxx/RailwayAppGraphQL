using System.ComponentModel.DataAnnotations;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Models.Tickets;

public sealed class Ticket
{
    public Guid Id { get; set; }
    
    [MaxLength(25)]
    public required string Number { get; set; }
    
    [MaxLength(100)]
    public required string PassengerName { get; set; }
    
    [MaxLength(50)]
    [EmailAddress]
    public string? PassengerEmail { get; set; }
    
    [MaxLength(10)]
    public required string SeatNumber { get; set; }

    public decimal Price { get; set; }
    public Currency Currency { get; set; } = Currency.USD;
    
    public DateTime PurchasedAtUtc { get; set; } = DateTime.UtcNow;
    
    // Foreign key to Train
    public Guid TrainId { get; set; }
    // Navigation property
    public Train Train { get; set; } = null!;
}