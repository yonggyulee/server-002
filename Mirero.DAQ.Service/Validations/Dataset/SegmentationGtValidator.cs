using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class SegmentationGtValidator : BaseValidator<SegmentationGt>
{
    public SegmentationGtValidator(IConfiguration configuration) : base(configuration)
    {
    }
}