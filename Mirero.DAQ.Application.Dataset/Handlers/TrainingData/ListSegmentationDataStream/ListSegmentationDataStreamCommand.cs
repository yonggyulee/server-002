using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSegmentationDataStream;

public class ListSegmentationDataStreamCommand : IRequest<ListSegmentationDataStreamResponse>
{
    public ListSegmentationDataStreamRequest Request { get; set; }
    public IServerStreamWriter<ListSegmentationDataStreamResponse> ResponseStream { get; set; }

    public ListSegmentationDataStreamCommand(ListSegmentationDataStreamRequest request,
        IServerStreamWriter<ListSegmentationDataStreamResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}