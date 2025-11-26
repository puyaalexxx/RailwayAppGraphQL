using RailwayAppGraphQL.Models.Trains;
using Type = RailwayAppGraphQL.Models.Trains.Type;

namespace RailwayAppGraphQL.GraphQL.Inputs.Trains;

public record UpdateTrainInput(
    string? Name,
    string? Number,
    Type? Type,
    int? Seats,
    Status? Status
);