using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class SampleValidator : BaseValidator<Sample>
{
    public SampleValidator(IConfiguration configuration) : base(configuration)
    {
    }
}