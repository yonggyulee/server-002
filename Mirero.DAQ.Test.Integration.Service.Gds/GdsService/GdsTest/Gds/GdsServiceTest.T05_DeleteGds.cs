using System.Linq;
using System.Threading;
using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

public partial class GdsServiceTest
{
    [Fact]
    public async void T05_Delete_Gds()
    {
        var listGdsesResponse = await List_Gds();

        if (listGdsesResponse.Gdses.Count > 0)
        {
            var gds = listGdsesResponse.Gdses.FirstOrDefault();
            _client.DeleteGds(new DeleteGdsRequest
            {
                GdsId = gds.Id,
                LockTimeoutSec = 3
            }, cancellationToken: CancellationToken.None);

            listGdsesResponse = await List_Gds();
            Assert.Null(listGdsesResponse.Gdses.SingleOrDefault(x => x.Id == gds.Id));
        }
        else
        {
            await UploadGdsStream();
            _client.DeleteGds(new DeleteGdsRequest
            {
                GdsId = 1,
                LockTimeoutSec = 3
            }, cancellationToken: CancellationToken.None);

            listGdsesResponse = await List_Gds();
            Assert.True(listGdsesResponse.Gdses.Count == 0);
        }

        Assert.Throws<RpcException>(() =>
        {
            _client.DeleteGds(new DeleteGdsRequest
            {
                GdsId = 100
            }, cancellationToken: CancellationToken.None);
        });
    }
}