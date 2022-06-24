using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.ClipService;

public partial class ClipServiceTest
{
    [Fact]
    public async void T02_ClipLayers()
    {
        var window = new DoubleWindow
        {
            MinX = 200,
            MinY = 200,
            MaxX = 300000,
            MaxY = 300000
        };

        var response = await _client.ClipLayersAsync(new ClipLayersRequest
        {
            Window = window,
            Options =
            {
                new ClipOption
                {
                    Layer = "4:0",
                    HorizontalStretch = 1.0,
                    VerticalStretch = 1.0,
                    MergeLayer = false
                },
                new ClipOption
                {
                    Layer = "7:0",
                    HorizontalStretch = 1.0,
                    VerticalStretch = 1.0,
                    MergeLayer = false
                }
            }
        });

        Assert.NotNull(response);

    }
}