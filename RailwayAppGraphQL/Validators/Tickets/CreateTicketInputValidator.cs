using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Tickets;

namespace RailwayAppGraphQL.Validators.Tickets;

public sealed class CreateTicketInputValidator : AbstractValidator<CreateTicketInput>
{
    public CreateTicketInputValidator()
    {
        // Ticket number
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Ticket number is required.")
            .MaximumLength(25).WithMessage("Ticket number cannot exceed 25 characters.");

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

        // TrainId (foreign key)
        RuleFor(x => x.TrainId)
            .NotEmpty().WithMessage("TrainId is required.");
    }
}