using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ImageDatasetValidator : BaseValidator<ImageDataset>
{
    public ImageDatasetValidator(IConfiguration configuration) : base(configuration)
    {
    }
}