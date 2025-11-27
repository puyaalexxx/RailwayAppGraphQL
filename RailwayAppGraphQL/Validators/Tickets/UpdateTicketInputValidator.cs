using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;

namespace RailwayAppGraphQL.Validators.Tickets;

public sealed class UpdateTicketInputValidator : AbstractValidator<UpdateTicketInput>
{
    private readonly IDbContextFactory<ApplicationDbContext> _factory;
    
    public UpdateTicketInputValidator(IDbContextFactory<ApplicationDbContext> factory)
    {
        _factory = factory;
        
        // Ticket number
        RuleFor(x => x.Number)
            .MaximumLength(25).WithMessage("Ticket number cannot exceed 25 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Number));

        // Passenger name
        RuleFor(x => x.PassengerName)
            .MaximumLength(100).WithMessage("Passenger name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.PassengerName));

        // Passenger email (optional, but must be valid if provided)
        RuleFor(x => x.PassengerEmail)
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.")
            .EmailAddress().WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrWhiteSpace(x.PassengerEmail));

        // Seat number
        RuleFor(x => x.SeatNumber)
            .MaximumLength(10).WithMessage("Seat number cannot exceed 10 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.SeatNumber));

        // Price
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.")
            .When(x => x.Price.HasValue);

        // Currency
        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Currency must be a valid value.")
            .When(x => x.Currency.HasValue);

        RuleFor(x => x.PurchasedAtUtc)
            .Must(dt => dt.HasValue && dt.Value.Kind == DateTimeKind.Utc)
            .WithMessage("PurchasedAtUtc must be UTC.")
            .When(x => x.PurchasedAtUtc.HasValue);

        // TrainId (foreign key)
        RuleFor(x => x.TrainId)
            .MustAsync(TrainExists)
            .When(x => x.TrainId.HasValue)
            .WithMessage("Train ID does not exist, please provide an existing one.");
    }
    
    private async Task<bool> TrainExists(Guid? trainId, CancellationToken cancellationToken)
    {
        if (trainId is null)
            return true;

        await using var db = await _factory.CreateDbContextAsync(cancellationToken);

        return await db.Trains.AnyAsync(s => s.Id == trainId, cancellationToken);
    }
}