using FluentValidation.Results;

namespace Mirero.DAQ.Service.Interceptors.Validations;

public interface IValidatorErrorMessageHandler
{
    Task<string> HandleAsync(IList<ValidationFailure> failures);
}