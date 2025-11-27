namespace RailwayAppGraphQL.GraphQL.Inputs.Stops;

public record CreateStopInput(
    Guid StationId,
    Guid TrainId,
    DateTime DepartureTimeUtc,
    DateTime ArrivalTimeUtc);