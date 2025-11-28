using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;

namespace RailwayAppGraphQL.Validators.Tickets;

public sealed class CreateTicketInputValidator : AbstractValidator<CreateTicketInput>
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;

    public CreateTicketInputValidator(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;

        // Ticket number
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Ticket number is required.")
            .MaximumLength(25).WithMessage("Ticket number cannot exceed 25 characters.")
            .MustAsync(TicketNumberUniqueForTrain)
            .WithMessage("A ticket with this number already exists for the selected train.");

        // Passenger name
        RuleFor(x => x.PassengerName)
            .NotEmpty().WithMessage("Passenger name is required.")
            .MaximumLength(100).WithMessage("Passenger name cannot exceed 100 characters.");

        // Passenger email (optional, but must be valid if provided)
        RuleFor(x => x.PassengerEmail)
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.")
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrWhiteSpace(x.PassengerEmail));

        // Seat number
        RuleFor(x => x.SeatNumber)
            .NotEmpty().WithMessage("Seat number is required.")
            .MaximumLength(10).WithMessage("Seat number cannot exceed 10 characters.");

        // Price
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");

        // Currency
        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Currency must be a valid value.");

        RuleFor(x => x.PurchasedAtUtc.Kind)
            .Equal(DateTimeKind.Utc)
            .WithMessage("PurchasedAtUtc must be UTC.");

        // TrainId must exist
        RuleFor(x => x.TrainId)
            .MustAsync(TrainExists)
            .WithMessage("Train ID does not exist, please provide an existing one.");

        // Train must have stops
        RuleFor(x => x.TrainId)
            .MustAsync(TrainHasStops)
            .WithMessage("The selected train has no scheduled stops and cannot be used for ticket creation.");
    }

    // Async method for checking uniqueness
    private async Task<bool> TicketNumberUniqueForTrain(CreateTicketInput input, string ticketNumber,
        CancellationToken cancellationToken)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        // Query the database for an existing ticket with the same train and number
        var exists = await db.Tickets
            .AsNoTracking() // VERY IMPORTANT: avoid EF tracking
            .AnyAsync(t => t.TrainId == input.TrainId && t.Number == ticketNumber, cancellationToken);

        return !exists;
    }

    private async Task<bool> TrainExists(Guid trainId, CancellationToken cancellationToken)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Trains.AnyAsync(s => s.Id == trainId, cancellationToken);
    }

    private async Task<bool> TrainHasStops(Guid trainId, CancellationToken cancellationToken)
    {
        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        var train = await db.Trains
            .Include(t => t.Stops)
            .FirstOrDefaultAsync(t => t.Id == trainId, cancellationToken);

        // Return true if train exists and has at least one stop
        return train != null && train.Stops.Any();
    }
}