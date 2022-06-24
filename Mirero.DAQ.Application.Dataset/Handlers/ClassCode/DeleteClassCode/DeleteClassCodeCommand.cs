using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCode;

public class DeleteClassCodeCommand : IRequest<Domain.Dataset.Protos.V1.ClassCode>
{
    public DeleteClassCodeRequest Request { get; set; }

    public DeleteClassCodeCommand(DeleteClassCodeRequest request)
    {
        Request = request;
    }
}