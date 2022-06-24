using Mirero.DAQ.Domain.Gds.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1.Worker;
using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Worker.Gds.GdsService;

public partial class GdsServiceTest
{
    [Fact]
    public async void T03_GetGdsProperty()
    {
        var response = await _client.GetGdsPropertyAsync(new Empty());

        Assert.NotNull(response);
    }
}