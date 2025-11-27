using FluentValidation;
using RailwayAppGraphQL.GraphQL.Inputs.Stations;

namespace RailwayAppGraphQL.Validators.Stations;

public sealed class UpdateStationInputValidator : AbstractValidator<UpdateStationInput>
{
    public UpdateStationInputValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Station name must not exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Address)
            .MaximumLength(150).WithMessage("Station address cannot exceed 150 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}