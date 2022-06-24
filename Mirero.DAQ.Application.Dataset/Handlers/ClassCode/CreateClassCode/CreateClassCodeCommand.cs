using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.CreateClassCode;

public class CreateClassCodeCommand : IRequest<Domain.Dataset.Protos.V1.ClassCode>
{
    public CreateClassCodeRequest Request { get; set; }

    public CreateClassCodeCommand(CreateClassCodeRequest request)
    {
        Request = request;
    }

}