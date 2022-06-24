using FluentValidation.Results;

namespace Mirero.DAQ.Service.Interceptors.Validations;

internal static class ValidationFailureExtensions
{
    public static IEnumerable<ValidationTrailers> ToValidationTrailers(this IList<ValidationFailure> failures)
    {
        return failures.Select(x => new ValidationTrailers {
            PropertyName = x.PropertyName,
            AttemptedValue = x.AttemptedValue?.ToString(),
            ErrorMessage = x.ErrorMessage
        }).ToList();
    }
}