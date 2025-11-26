namespace RailwayAppGraphQL.Models;

public sealed class Stop
{
    public Guid Id { get; set; }

    public DateTime DepartureTimeUtc { get; set; }

    public DateTime ArrivalTimeUtc { get; set; }

    // Foreign key
    [GraphQLIgnore] public Guid StationId { get; set; }

    // Navigation property (linked to Station)
    public Station Station { get; set; } = null!;
}