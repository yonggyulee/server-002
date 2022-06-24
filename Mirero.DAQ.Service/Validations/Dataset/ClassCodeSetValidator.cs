using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ClassCodeSetValidator : BaseValidator<ClassCodeSet>
{
    public ClassCodeSetValidator(IConfiguration configuration) : base(configuration)
    {
    }
}