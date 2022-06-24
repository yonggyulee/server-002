using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ObjectDetectionGtDatasetValidator : BaseValidator<ObjectDetectionGtDataset>
{
    public ObjectDetectionGtDatasetValidator(IConfiguration configuration) : base(configuration)
    {
    }
}