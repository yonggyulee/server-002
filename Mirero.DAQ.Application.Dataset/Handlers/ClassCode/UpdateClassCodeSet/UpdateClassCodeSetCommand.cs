using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.UpdateClassCodeSet;

public class UpdateClassCodeSetCommand : IRequest<ClassCodeSet>
{
    public UpdateClassCodeSetRequest Request { get; set; }

    public UpdateClassCodeSetCommand(UpdateClassCodeSetRequest request)
    {
        Request = request;
    }
}