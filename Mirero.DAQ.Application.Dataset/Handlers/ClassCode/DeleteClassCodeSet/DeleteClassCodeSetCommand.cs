using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCodeSet;

public class DeleteClassCodeSetCommand : IRequest<ClassCodeSet>
{
    public DeleteClassCodeSetRequest Request { get; set; }

    public DeleteClassCodeSetCommand(DeleteClassCodeSetRequest request)
    {
        Request = request;
    }
}