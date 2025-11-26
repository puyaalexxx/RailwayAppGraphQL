namespace RailwayAppGraphQL.GraphQL.Inputs.Stations;

public sealed record UpdateStationInput(
    string Name,
    string Address,
    bool HasWc,
    bool HasCoffeeMachine,
    bool HasWaitingRoom);