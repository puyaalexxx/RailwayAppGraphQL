using System.ComponentModel.DataAnnotations;
using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.Models.Trains;

public sealed class Train
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(15)]
    public required string Number { get; set; }

    public Type Type { get; set; }

    public int Seats { get; set; }
   
    public Status Status { get; set; }
    
    // Navigation Properties
    public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}