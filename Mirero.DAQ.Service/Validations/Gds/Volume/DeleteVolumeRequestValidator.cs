using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Volume;

public class DeleteVolumeRequestValidator : AbstractValidator<DeleteVolumeRequest>
{
    public DeleteVolumeRequestValidator()
    {
        RuleFor(x => x.VolumeId).NotEmpty();
    }
}