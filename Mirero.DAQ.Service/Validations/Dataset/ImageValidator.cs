using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Dataset;

public class ImageValidator : BaseValidator<Image>
{
    public ImageValidator(IConfiguration configuration) : base(configuration)
    {
    }
}