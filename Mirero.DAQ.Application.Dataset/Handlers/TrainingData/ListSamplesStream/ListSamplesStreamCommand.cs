using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.TrainingData.ListSamplesStream;

public class ListSamplesStreamCommand : IRequest<ListSamplesStreamResponse>
{
    public ListSamplesStreamRequest Request { get; set; }
    public IServerStreamWriter<ListSamplesStreamResponse> ResponseStream { get; set; }

    public ListSamplesStreamCommand(ListSamplesStreamRequest request, IServerStreamWriter<ListSamplesStreamResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}