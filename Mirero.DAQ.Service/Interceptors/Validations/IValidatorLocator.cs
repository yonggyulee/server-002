using FluentValidation;

namespace Mirero.DAQ.Service.Interceptors.Validations;

public interface IValidatorLocator
{
    bool TryGetValidator<TRequest>(out IValidator<TRequest> result) where TRequest : class;
}