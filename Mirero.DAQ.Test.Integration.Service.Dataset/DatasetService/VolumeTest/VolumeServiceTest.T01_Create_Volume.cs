using Grpc.Core;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T01_Create_Volume()
    {
        var superUserOptions = await _GetAuthAsync();

        var tempDir = $"{_fixture.VolumeBaseUri}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "ID_Volume1",
            Title = "Title_Volume1",
            Type = "image", // TODO ENUM
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume1"
        };

        try
        {
            var createdVolume = _client.CreateVolume(createVolumeRequest, superUserOptions);
            _output.WriteLine(createdVolume.ToString());
            Assert.NotNull(createdVolume.ToString());
        }
        catch
        {
            await Assert.ThrowsAnyAsync<RpcException>(async () =>
            {
                var createdVolume = await _client.CreateVolumeAsync(createVolumeRequest, superUserOptions);
                _output.WriteLine(createdVolume.ToString());
                Assert.NotNull(createdVolume.ToString());
            });
        }
    }
}