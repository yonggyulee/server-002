using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListClassificationDataStream;

public class ListClassificationDataStreamCommand : IRequest
{
    public ListClassificationDataStreamRequest Request { get; set; }
    public IServerStreamWriter<ListClassificationDataStreamResponse> ResponseStream { get; set; }

    public ListClassificationDataStreamCommand(ListClassificationDataStreamRequest request,
        IServerStreamWriter<ListClassificationDataStreamResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}