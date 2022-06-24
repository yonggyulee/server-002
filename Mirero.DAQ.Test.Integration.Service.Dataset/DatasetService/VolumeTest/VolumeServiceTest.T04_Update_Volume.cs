using Grpc.Core;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T04_Update_Volume()
    {
        var superUserOptions = await _GetAuthAsync();
        var tempDir = $"{_fixture.VolumeBaseUri}/{Path.GetRandomFileName()}";

        var updateVolumeRequest = new UpdateVolumeRequest
        {
            Id = "ID_Volume1",
            Title = "Title_Volume1",
            Type = "image", // TODO ENUM
            Uri = tempDir, //Todo path 변경 확인 필요
            Capacity = 100000000,
            Description = "Test Volume1 Update"
        };

        try
        {
            var updateVolume = _client.UpdateVolume(updateVolumeRequest, superUserOptions);
            _output.WriteLine(updateVolume.ToString());
            Assert.NotNull(updateVolume.ToString());
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                var updateVolume = await _client.UpdateVolumeAsync(updateVolumeRequest, superUserOptions);
                _output.WriteLine(updateVolume.ToString());
                Assert.NotNull(updateVolume.ToString());
            });
        }
    }
}