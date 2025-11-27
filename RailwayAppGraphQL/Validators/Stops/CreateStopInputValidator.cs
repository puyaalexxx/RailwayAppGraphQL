using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.GraphQL.Inputs.Stops;

namespace RailwayAppGraphQL.Validators.Stops;

public class CreateStopInputValidator : AbstractValidator<CreateStopInput>
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public CreateStopInputValidator(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;

        RuleFor(x => x.ArrivalTimeUtc)
            .GreaterThan(x => x.DepartureTimeUtc)
            .WithMessage("Arrival time must be after departure time.");

        RuleFor(x => x.DepartureTimeUtc.Kind)
            .Equal(DateTimeKind.Utc)
            .WithMessage("DepartureTimeUtc must be UTC.");

        RuleFor(x => x.ArrivalTimeUtc.Kind)
            .Equal(DateTimeKind.Utc)
            .WithMessage("ArrivalTimeUtc must be UTC.");

        RuleFor(x => x.StationId)
            .MustAsync(StationExists)
            .WithMessage("Station ID does not exist, please provide an existing one.");

        RuleFor(x => x.TrainId)
            .MustAsync(TrainExists)
            .WithMessage("Train ID does not exist, please provide an existing one.");
    }

    private async Task<bool> StationExists(Guid stationId, CancellationToken cancellationToken)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Stations.AnyAsync(s => s.Id == stationId, cancellationToken);
    }


    private async Task<bool> TrainExists(Guid trainId, CancellationToken cancellationToken)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Trains.AnyAsync(s => s.Id == trainId, cancellationToken);
    }
}