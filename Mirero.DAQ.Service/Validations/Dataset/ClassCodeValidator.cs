using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ClassCodeValidator : BaseValidator<ClassCode>
{
    public ClassCodeValidator(IConfiguration configuration) : base(configuration)
    {
    }
}