using System.IO;
using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T04_Delete_Volume()
    {
        var superUserOptions = await _GetAuthAsync();

        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "DeleteVolumeTestId",
            Title = "DeleteVolumeTest",
            Type = "gds",
            Uri = tempDir,
            Capacity = 10000,
            Description = "DeleteVolumeTest"
        };
        _client.CreateVolume(createVolumeRequest, superUserOptions);

        var deleteVolumeRequest = new DeleteVolumeRequest
        {
            VolumeId = "TestId"
        };

        Assert.Throws<RpcException>(() => { _client.DeleteVolume(deleteVolumeRequest, superUserOptions); });

        deleteVolumeRequest.VolumeId = "DeleteVolumeTestId";
        _client.DeleteVolume(deleteVolumeRequest, superUserOptions);
    }
}