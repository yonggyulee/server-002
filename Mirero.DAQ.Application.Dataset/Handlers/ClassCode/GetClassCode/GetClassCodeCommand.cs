using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCode;

public class GetClassCodeCommand : IRequest<Domain.Dataset.Protos.V1.ClassCode>
{
    public GetClassCodeRequest Request { get; set; }

    public GetClassCodeCommand(GetClassCodeRequest request)
    {
        Request = request;
    }
}