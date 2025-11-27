namespace RailwayAppGraphQL.GraphQL.Inputs.Stops;

public sealed record UpdateStopInput(
    Guid? StationId,
    Guid? TrainId,
    DateTime? DepartureTimeUtc,
    DateTime? ArrivalTimeUtc);