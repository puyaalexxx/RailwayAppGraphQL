using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.GraphQL.Inputs.Stops;

namespace RailwayAppGraphQL.Validators.Stops;

public class UpdateStopInputValidator : AbstractValidator<UpdateStopInput>
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public UpdateStopInputValidator(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;

        RuleFor(x => x.ArrivalTimeUtc)
            .GreaterThan(x => x.DepartureTimeUtc)
            .WithMessage("Arrival time must be after departure time.")
            .When(x => x.ArrivalTimeUtc.HasValue && x.DepartureTimeUtc.HasValue);

        RuleFor(x => x.DepartureTimeUtc)
            .Must(dt => dt.HasValue && dt.Value.Kind == DateTimeKind.Utc)
            .WithMessage("DepartureTimeUtc must be UTC.")
            .When(x => x.DepartureTimeUtc.HasValue);

        RuleFor(x => x.ArrivalTimeUtc)
            .Must(dt => dt.HasValue && dt.Value.Kind == DateTimeKind.Utc)
            .WithMessage("ArrivalTimeUtc must be UTC.")
            .When(x => x.ArrivalTimeUtc.HasValue);

        RuleFor(x => x.StationId)
            .MustAsync(StationExists)
            .When(x => x.StationId.HasValue)
            .WithMessage("Station does not exist.");

        RuleFor(x => x.TrainId)
            .MustAsync(TrainExists)
            .When(x => x.TrainId.HasValue)
            .WithMessage("Train ID does not exist, please provide an existing one.");
    }

    private async Task<bool> StationExists(Guid? stationId, CancellationToken cancellationToken)
    {
        if (stationId is null)
            return true;

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Stations.AnyAsync(s => s.Id == stationId, cancellationToken);
    }

    private async Task<bool> TrainExists(Guid? trainId, CancellationToken cancellationToken)
    {
        if (trainId is null)
            return true;

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Trains.AnyAsync(s => s.Id == trainId, cancellationToken);
    }
}