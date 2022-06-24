using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Gds;

public class DeleteGdsRequestValidator : AbstractValidator<DeleteGdsRequest>
{
    public DeleteGdsRequestValidator()
    {
        RuleFor(x => x.GdsId).NotEmpty();
    }
}