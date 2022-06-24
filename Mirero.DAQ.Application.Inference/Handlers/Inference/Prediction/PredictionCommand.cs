using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Inference.Prediction;

public class PredictionCommand : IRequest<PredictionResponse>
{
    public PredictionRequest Request { get; set; }

    public PredictionCommand(PredictionRequest request)
    {
        Request = request;
    }
}