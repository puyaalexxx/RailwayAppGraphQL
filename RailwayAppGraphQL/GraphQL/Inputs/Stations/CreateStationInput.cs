namespace RailwayAppGraphQL.GraphQL.Inputs.Stations;

public sealed record CreateStationInput(
    string Name,
    string Address,
    bool HasWc,
    bool HasCoffeeMachine,
    bool HasWaitingRoom);