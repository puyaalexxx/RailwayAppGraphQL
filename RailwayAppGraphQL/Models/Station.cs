namespace RailwayAppGraphQL.Models;

public sealed class Station
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }

    public required string Address { get; set; }

    public bool HasWc { get; set; }
    
    public bool HasCoffeeMachine { get; set; }
    
    public bool HasWaitingRoom { get; set; }
}