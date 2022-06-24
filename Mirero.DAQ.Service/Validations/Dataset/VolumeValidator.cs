using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class VolumeValidator : BaseValidator<Volume>
{
    public VolumeValidator(IConfiguration configuration) : base(configuration)
    {
    }
}