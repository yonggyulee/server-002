using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ClassCodeReferenceImageValidator : BaseValidator<ClassCodeReferenceImage>
{
    public ClassCodeReferenceImageValidator(IConfiguration configuration) : base(configuration)
    {
    }
}