using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.FloorPlan;

public partial class FloorPlanServiceTest
{
    [Fact]
    public async void T01_Create_FloorPlan()
    {
        var superUserOption = await _GetAuthAsync();

        List<Layer> layers = new List<Layer>
        {
            new Layer
            {
                DataType = "abc",
                Name = "123",
                Layer_ = "qwe"
            },
            new Layer
            {
                DataType = "ABC", 
                Name = "456",
                Layer_ = "QWE"
            }
        };

        var floorPlanGdsData = new FloorPlanGdsData()
        {
            GdsId = 1,
            Layers = {layers},
            OffsetX = 1000,
            OffsetY = 1000
        };

        var createFloorPlanRequest = new CreateFloorPlanRequest
        {
           Gdses = { floorPlanGdsData },
           Title = "FloorPlan_1",
           Properties = "123",
           Description = "123"
        };

        var createFloorPlan = _client.CreateFloorPlan(createFloorPlanRequest, superUserOption);
        
        _output.WriteLine(createFloorPlan.ToString());
        Assert.NotNull(createFloorPlan.ToString());

    }
}

