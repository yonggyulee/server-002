using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Service.Validations.Account;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class ModelValidatorExtension
{
    public static IServiceCollection AddAccountValidator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IValidator<User>, UserValidator>();

        return services;
    }
}