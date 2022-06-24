using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.ClassCode.DeleteClassCodeReferenceImage;

public class DeleteClassCodeReferenceImageCommand : IRequest<ClassCodeReferenceImage>
{
    public DeleteClassCodeReferenceImageRequest Request { get; set; }

    public DeleteClassCodeReferenceImageCommand(DeleteClassCodeReferenceImageRequest request)
    {
        Request = request;
    }
}