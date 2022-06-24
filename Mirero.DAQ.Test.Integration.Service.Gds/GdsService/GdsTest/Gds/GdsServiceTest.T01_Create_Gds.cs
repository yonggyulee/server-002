using System.IO;
using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

public partial class GdsServiceTest
{
    [Fact]
    public async void T01_Create_Gds()
    {
        var superUserOption = await _GetAuthAsync(); 
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "Mirero",
            Title = "GdsTestVolume",
            Type = "gds", 
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume1"
        };

        _volume.CreateVolume(createVolumeRequest, superUserOption);

        var createGdsRequest = new CreateGdsRequest
        {
            VolumeId = "Mirero",
            Filename = "GdsFileName",
            Extension = "gds",
            Properties = "1",
            Description = "CreateGds"
        };

        var createGdsResponse = _client.CreateGds(createGdsRequest, superUserOption);

        Assert.NotNull(createGdsResponse);

        createGdsRequest.VolumeId = "NotExist";

        Assert.Throws<RpcException>(() => { _client.CreateGds(createGdsRequest, superUserOption); });
    }
}

