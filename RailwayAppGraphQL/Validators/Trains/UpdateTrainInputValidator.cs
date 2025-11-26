using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Trains;

namespace RailwayAppGraphQL.Validators.Trains;

public class UpdateTrainInputValidator : AbstractValidator<UpdateTrainInput>
{
    public UpdateTrainInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Train name is required.")
            .MaximumLength(50).WithMessage("Train name must not exceed 50 characters.");

        RuleFor(x => x.Number)
            .MaximumLength(15).WithMessage("Train number must not exceed 15 characters.")
            .Matches(@"^[A-Z0-9]+$").When(x => x.Number != null)
            .WithMessage("Train number must be alphanumeric.");

        RuleFor(x => x.Seats).GreaterThan(0).When(x => x.Seats.HasValue)
            .WithMessage("Seats must be greater than 0.");
        
        // Type: must be a valid enum
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Train type must be valid.");

        // Status: must be a valid enum
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Train status must be valid.");
    }
}