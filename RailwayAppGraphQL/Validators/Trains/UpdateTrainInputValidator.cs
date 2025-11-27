using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Trains;

namespace RailwayAppGraphQL.Validators.Trains;

public class UpdateTrainInputValidator : AbstractValidator<UpdateTrainInput>
{
    public UpdateTrainInputValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Train name must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Number)
            .MaximumLength(15).WithMessage("Train number must not exceed 15 characters.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Train number must be alphanumeric.")
            .When(x => !string.IsNullOrWhiteSpace(x.Number));

        RuleFor(x => x.Seats)
            .GreaterThan(0).WithMessage("Seats must be greater than 0.")
            .When(x => x.Seats.HasValue);
        
        // Type: must be a valid enum
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Train type must be valid.")
            .When(x => x.Type.HasValue);

        // Status: must be a valid enum
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Train status must be valid.")
            .When(x => x.Status.HasValue);
    }
}