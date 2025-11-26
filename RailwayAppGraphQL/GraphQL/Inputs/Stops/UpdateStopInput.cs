namespace RailwayAppGraphQL.GraphQL.Inputs.Stops;

public sealed record UpdateStopInput(
    Guid StationId,
    DateTime DepartureTimeUtc,
    DateTime ArrivalTimeUtc);