using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Gds;

public class UploadGdsRequestValidator : AbstractValidator<UploadGdsStreamRequest>
{
    public UploadGdsRequestValidator()
    {
        RuleFor(x => x.GdsId).NotEmpty();
    }
}