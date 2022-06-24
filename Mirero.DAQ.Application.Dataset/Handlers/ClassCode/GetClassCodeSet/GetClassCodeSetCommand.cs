using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.GetClassCodeSet;

public class GetClassCodeSetCommand : IRequest<ClassCodeSet>
{
    public GetClassCodeSetRequest Request { get; set; }

    public GetClassCodeSetCommand(GetClassCodeSetRequest request)
    {
        Request = request;
    }
}