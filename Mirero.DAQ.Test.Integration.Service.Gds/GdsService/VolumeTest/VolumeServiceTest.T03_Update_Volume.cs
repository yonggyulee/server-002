using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;
using System.IO;
using Grpc.Core;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T03_Update_Volume()
    {
        var superUserOptions = await _GetAuthAsync();
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "UpdateVolumeTestId",
            Title = "UpdateVolumeTest",
            Type = "gds",
            Uri = tempDir,
            Capacity = 10000,
            Description = "UpdateVolumeTest"
        };
        _client.CreateVolume(createVolumeRequest, superUserOptions);

        tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";

        var updateVolumeRequest = new UpdateVolumeRequest
        {
            Id = "UpdateVolumeTestId",
            Title = "Changed",
            Type = "gds",
            Uri = tempDir,
            Capacity = 500,
            Description = "Test Update Success"
        };

        var updatedVolume = _client.UpdateVolume(updateVolumeRequest, superUserOptions);

        Assert.NotNull(updatedVolume);
        Assert.Equal(updatedVolume.Id, updateVolumeRequest.Id);
        Assert.Equal(updatedVolume.Title, updateVolumeRequest.Title);
        Assert.Equal(updatedVolume.Type, updateVolumeRequest.Type);
        Assert.Equal(updatedVolume.Uri, updateVolumeRequest.Uri);
        Assert.Equal(updatedVolume.Capacity, updateVolumeRequest.Capacity);
        Assert.Equal(updatedVolume.Description, updateVolumeRequest.Description);

        var existUpdatedDir = Directory.Exists(updatedVolume.Uri);
        Assert.True(existUpdatedDir);

        updateVolumeRequest.Id = "3";
        Assert.Throws<RpcException>(() => { _client.UpdateVolume(updateVolumeRequest, superUserOptions); });

        updateVolumeRequest.Id = string.Empty;
        Assert.Throws<RpcException>(() => { _client.UpdateVolume(updateVolumeRequest, superUserOptions); });

        updateVolumeRequest.Id = "UpdateVolumeTestId";
        updateVolumeRequest.Title = string.Empty;
        Assert.Throws<RpcException>(() => { _client.UpdateVolume(updateVolumeRequest, superUserOptions); });

        updateVolumeRequest.Title = string.Empty;
        updateVolumeRequest.Type = "gds";
        Assert.Throws<RpcException>(() => { _client.UpdateVolume(updateVolumeRequest, superUserOptions); });
    }
}