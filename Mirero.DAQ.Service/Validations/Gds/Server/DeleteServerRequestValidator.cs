using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Server;

public class DeleteServerRequestValidator : AbstractValidator<DeleteServerRequest>
{
    public DeleteServerRequestValidator()
    {
        RuleFor(x => x.ServerId).NotEmpty().Length(1, 256);
    }
}