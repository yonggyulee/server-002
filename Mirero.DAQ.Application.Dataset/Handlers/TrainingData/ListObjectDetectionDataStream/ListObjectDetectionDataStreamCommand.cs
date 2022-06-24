using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListObjectDetectionDataStream;

public class ListObjectDetectionDataStreamCommand : IRequest<ListObjectDetectionDataStreamResponse>
{
    public ListObjectDetectionDataStreamRequest Request { get; set; }
    public IServerStreamWriter<ListObjectDetectionDataStreamResponse> ResponseStream { get; set; }

    public ListObjectDetectionDataStreamCommand(ListObjectDetectionDataStreamRequest request,
        IServerStreamWriter<ListObjectDetectionDataStreamResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}