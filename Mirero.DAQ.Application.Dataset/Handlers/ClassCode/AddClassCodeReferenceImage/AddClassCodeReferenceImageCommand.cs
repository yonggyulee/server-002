using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.AddClassCodeReferenceImage;

public class AddClassCodeReferenceImageCommand : IRequest<ClassCodeReferenceImage>
{
    public ClassCodeReferenceImage Request { get; set; }

    public AddClassCodeReferenceImageCommand(ClassCodeReferenceImage request)
    {
        Request = request;
    }
}