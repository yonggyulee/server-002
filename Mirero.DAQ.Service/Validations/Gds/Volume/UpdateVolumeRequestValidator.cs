using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Volume;

public class UpdateVolumeRequestValidator : AbstractValidator<UpdateVolumeRequest>
{
    public UpdateVolumeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(1, 256);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Type).NotEmpty().Length(1, 256);
        RuleFor(x => x.Uri).NotEmpty().Length(1, 512);
        RuleFor(x => x.Capacity).NotEmpty().GreaterThan(0);
    }
}