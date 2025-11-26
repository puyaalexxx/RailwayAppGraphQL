using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Stations;

namespace RailwayAppGraphQL.Validators.Stations;

public sealed class CreateStationInputValidator : AbstractValidator<CreateStationInput>
{
    public CreateStationInputValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Station name is required.")
            .MaximumLength(50).WithMessage("Station name must not exceed 50 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Station address is required.")
            .MaximumLength(150).WithMessage("Station address cannot exceed 150 characters.");
    }
}