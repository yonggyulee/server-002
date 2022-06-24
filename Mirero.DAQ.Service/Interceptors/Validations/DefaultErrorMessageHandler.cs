using FluentValidation.Results;

namespace Mirero.DAQ.Service.Interceptors.Validations;

internal class DefaultErrorMessageHandler : IValidatorErrorMessageHandler
{
    public Task<string> HandleAsync(IList<ValidationFailure> failures)
    {
        var errors = failures
            .Select(f => $"{f.PropertyName} : {f.ErrorMessage}")
            .ToList();

        return Task.FromResult(string.Join("\n", errors));
    }
}