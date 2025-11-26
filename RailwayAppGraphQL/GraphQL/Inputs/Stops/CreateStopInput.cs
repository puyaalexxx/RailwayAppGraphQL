namespace RailwayAppGraphQL.GraphQL.Inputs.Stops;

public record CreateStopInput(
    Guid StationId,
    DateTime DepartureTimeUtc,
    DateTime ArrivalTimeUtc);