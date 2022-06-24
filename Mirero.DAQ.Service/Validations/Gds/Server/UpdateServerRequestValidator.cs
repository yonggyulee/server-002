using System.Net;
using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Server;

public class UpdateServerRequestValidator : AbstractValidator<UpdateServerRequest>
{
    public UpdateServerRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Length(1, 256);
        RuleFor(x => x.Address).Must(x => IPAddress.TryParse(x, out _)).WithMessage("유효하지 않은 IP주소 입니다.");
        RuleFor(x => x.OsType).NotEmpty().Length(1, 256);
        RuleFor(x => x.OsVersion).NotEmpty().Length(1, 256);
        RuleFor(x => x.CpuCount).NotEmpty();
        RuleFor(x => x.CpuMemory).NotEmpty();
    }
}