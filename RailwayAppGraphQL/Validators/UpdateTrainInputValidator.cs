using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs;

namespace RailwayAppGraphQL.Validators;

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
    }
}