using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ClassificationGtValidator : BaseValidator<ClassificationGt>
{
    public ClassificationGtValidator(IConfiguration configuration) : base(configuration)
    {
    }
}