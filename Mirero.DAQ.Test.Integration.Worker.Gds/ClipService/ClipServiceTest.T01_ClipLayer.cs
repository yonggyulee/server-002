using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ClipService;

public partial class ClipServiceTest
{
    [Fact]
    public async void T01_ClipLayer()
    {
        var window = new DoubleWindow
        {
            MinX = 1000,
            MinY = 1000,
            MaxX = 3000000,
            MaxY = 3000000
        };
        
        var option = new ClipOption
        {
            Layer = "4:0",
            HorizontalStretch = 1.0,
            VerticalStretch = 1.0,
            MergeLayer = false
        };

        var response = await _client.ClipLayerAsync(new ClipLayerRequest
        {
            Window = window,
            Option = option
        });

        Assert.NotNull(response);
    }
}