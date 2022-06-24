using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ObjectDetectionGtValidator : BaseValidator<ObjectDetectionGt>
{
    public ObjectDetectionGtValidator(IConfiguration configuration) : base(configuration)
    {
    }
}