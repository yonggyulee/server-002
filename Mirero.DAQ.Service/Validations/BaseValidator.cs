using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Mirero.DAQ.Service.Validations;

public class BaseValidator<T> : AbstractValidator<T> where T : class
{
    protected readonly IConfiguration Configuration;

    public BaseValidator(IConfiguration configuration)
    {
        Configuration = configuration ?? throw new ArgumentNullException();
    }
}