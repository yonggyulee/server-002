using FluentValidation;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Service.Validations.Gds.Gds;

public class DownLoadGdsRequestValidator : AbstractValidator<DownloadGdsStreamRequest>
{
    public DownLoadGdsRequestValidator()
    {
        RuleFor(x => x.GdsId).NotEmpty();
        RuleFor(x => x.ChunkSize).NotEmpty();
    }
}