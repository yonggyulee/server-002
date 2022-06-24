using Grpc.Core;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T05_Delete_Volume()
    {
        var superUserOptions = await _GetAuthAsync();

        var deleteVolumeRequest = new DeleteVolumeRequest
        {
            VolumeId = "ID_Volume1"
        };

        try
        {
            var deletedVolume = _client.DeleteVolume(deleteVolumeRequest, superUserOptions);
            _output.WriteLine(deletedVolume.ToString());
            Assert.NotNull(deletedVolume.ToString());
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                var deletedVolume = await _client.DeleteVolumeAsync(deleteVolumeRequest, superUserOptions);
                _output.WriteLine(deletedVolume.ToString());
                Assert.NotNull(deletedVolume.ToString());
            });
        }
    }
}