using System.ComponentModel.DataAnnotations;

namespace RailwayAppGraphQL.Models;

public sealed class Station
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(150)]
    public required string Address { get; set; }

    public bool HasWc { get; set; }
    
    public bool HasCoffeeMachine { get; set; }
    
    public bool HasWaitingRoom { get; set; }
}