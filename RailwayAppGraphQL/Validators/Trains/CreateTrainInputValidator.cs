using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Trains;

namespace RailwayAppGraphQL.Validators.Trains;

public sealed class CreateTrainInputValidator : AbstractValidator<CreateTrainInput>
{
    public CreateTrainInputValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Train name is required.")
            .MaximumLength(50).WithMessage("Train name must not exceed 50 characters.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Train number is required.")
            .MaximumLength(15).WithMessage("Train number must not exceed 15 characters.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Train number must be alphanumeric.");

        RuleFor(x => x.Seats)
            .GreaterThan(0).WithMessage("Seats must be greater than 0.");
        
        // Type: must be a valid enum
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Train type must be valid.");

        // Status: must be a valid enum
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Train status must be valid.");
    }
}