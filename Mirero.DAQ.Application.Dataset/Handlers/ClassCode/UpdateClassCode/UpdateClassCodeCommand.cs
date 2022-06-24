using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCode;

public class UpdateClassCodeCommand : IRequest<Domain.Dataset.Protos.V1.ClassCode>
{
    public UpdateClassCodeRequest Request { get; set; }

    public UpdateClassCodeCommand(UpdateClassCodeRequest request)
    {
        Request = request;
    }
}