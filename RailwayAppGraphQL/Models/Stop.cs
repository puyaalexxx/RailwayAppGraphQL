using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Models;

/// <summary>
///     a scheduled stop of a specific train at a specific station at specific times.
///     Example: Train T1 stops at Station S1 at 10:00–10:10.
///     Each Stop is linked to one Train and one Station.
/// </summary>
public sealed class Stop
{
    public Guid Id { get; set; }

    public DateTime DepartureTimeUtc { get; set; }

    public DateTime ArrivalTimeUtc { get; set; }

    // Foreign key
    [GraphQLIgnore] public Guid StationId { get; set; }

    // Navigation property (linked to Station)
    public Station Station { get; set; } = null!;

    // Foreign key to Train - each stop belong to one train
    public Guid TrainId { get; set; }
    public Train Train { get; set; } = null!;
}