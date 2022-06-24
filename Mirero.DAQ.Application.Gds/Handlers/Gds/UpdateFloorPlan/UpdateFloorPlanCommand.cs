using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.UpdateFloorPlan;

public class UpdateFloorPlanCommand : IRequest<FloorPlan>
{
    public UpdateFloorPlanRequest Request { get; set; }

    public UpdateFloorPlanCommand(UpdateFloorPlanRequest request)
    {
        Request = request;
    }
}