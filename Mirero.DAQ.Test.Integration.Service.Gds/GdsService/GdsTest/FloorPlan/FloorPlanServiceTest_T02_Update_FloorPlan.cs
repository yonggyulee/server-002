using System.Collections.Generic;
using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.FloorPlan;

public partial class FloorPlanServiceTest
{
    [Fact]
    public async void T02_Update_FloorPlan()
    {
        var superUserOption = await _GetAuthAsync();

        List<FloorPlanGdsData> floorPlanGdsData = new List<FloorPlanGdsData>
        {
            new FloorPlanGdsData
            {
                GdsId = 1,
                Layers = { },
                OffsetX = 1000,
                OffsetY = 1000
            },
            new FloorPlanGdsData
            {
                GdsId = 2,
                Layers = { },
                OffsetX = 3000,
                OffsetY = 3000
            }
        };

        var updateFloorPlanRequest = new UpdateFloorPlanRequest()
        {
            Gdses = { floorPlanGdsData },
            Title = "FloorPlan_1",
            Properties = "456",
            Description = "456"
        };

        var updateFloorPlan = _client.UpdateFloorPlan(updateFloorPlanRequest, superUserOption);

        _output.WriteLine(updateFloorPlan.ToString());
        Assert.NotNull(updateFloorPlan.ToString());
    }
}