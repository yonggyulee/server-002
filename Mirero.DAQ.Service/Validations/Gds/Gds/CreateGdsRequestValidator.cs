using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Gds;

public class CreateGdsRequestValidator : AbstractValidator<CreateGdsRequest>
{
    public CreateGdsRequestValidator()
    {
        RuleFor(x => x.VolumeId).NotEmpty().Length(1, 256);
        RuleFor(x => x.Filename).NotEmpty().Length(1, 512);
        RuleFor(x => x.Extension).NotEmpty().Length(1, 20);
        RuleFor(x => x.Properties);
        RuleFor(x => x.Description);
    }
}