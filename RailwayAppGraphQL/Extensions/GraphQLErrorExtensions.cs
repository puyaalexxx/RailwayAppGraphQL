// ReSharper disable InconsistentNaming

using FluentValidation.Results;
using Path = HotChocolate.Path;

namespace RailwayAppGraphQL.Extensions;

public static class GraphQLErrorExtensions
{
    /// <summary>
    ///     Maps FluentValidation errors to GraphQL errors
    /// </summary>
    public static IEnumerable<IError> ToGraphQLErrors(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(e => ErrorBuilder.New()
                .SetMessage(e.ErrorMessage)
                .SetCode("VALIDATION_ERROR")
                .SetPath(Path.Root.Append(e.PropertyName))
                .Build()
            );
        return errors;
    }
}