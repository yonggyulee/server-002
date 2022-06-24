using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCodeSet;

public class CreateClassCodeSetCommand : IRequest<ClassCodeSet>
{
    public CreateClassCodeSetRequest Request { get; set; }

    public CreateClassCodeSetCommand(CreateClassCodeSetRequest request)
    {
        Request = request;
    }
}