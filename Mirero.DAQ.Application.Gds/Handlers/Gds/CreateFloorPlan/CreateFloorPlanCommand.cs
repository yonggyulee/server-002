using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.CreateFloorPlan;

public class CreateFloorPlanCommand : IRequest<FloorPlan>
{
    public CreateFloorPlanRequest Request { get; private set; }

    public CreateFloorPlanCommand(CreateFloorPlanRequest request)
    {
        Request = request;
    }
}