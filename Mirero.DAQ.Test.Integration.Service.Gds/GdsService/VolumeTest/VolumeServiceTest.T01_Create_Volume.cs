using System;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;
using System.IO;
using System.Linq;
using Grpc.Core;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T01_Create_Volume()
    {
        var superUserOptions = await _GetAuthAsync();

        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "1",
            Title = "Title_Volume1",
            Type = "gds",
            Uri = tempDir,
            Capacity = 1000,
            Description = "Test Volume1"
        };
        var createdVolume = _client.CreateVolume(createVolumeRequest, superUserOptions);

        Assert.NotNull(createdVolume);
        Assert.Equal(createdVolume.Id, createVolumeRequest.Id);
        Assert.Equal(createdVolume.Title, createVolumeRequest.Title);
        Assert.Equal(createdVolume.Type, createVolumeRequest.Type);
        Assert.Equal(createdVolume.Uri, createVolumeRequest.Uri);
        Assert.Equal(createdVolume.Capacity, createVolumeRequest.Capacity);
        Assert.Equal(createdVolume.Properties, createVolumeRequest.Properties);
        Assert.Equal(createdVolume.Description, createVolumeRequest.Description);

        Assert.Throws<RpcException>(() => { _client.CreateVolume(createVolumeRequest, superUserOptions); });

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var testText = new string(Enumerable.Repeat(chars, 257)
            .Select(x => x[random.Next(x.Length)]).ToArray());

        var createVolumeRequestTestCase = new CreateVolumeRequest
        {
            Id = testText,
            Title = "Title_Volume1",
            Type = testText,
            Uri = tempDir,
            Capacity = 1000,
            Description = "Test Volume1"
        };
        Assert.Throws<RpcException>(() => { _client.CreateVolume(createVolumeRequestTestCase, superUserOptions); });

        var createVolumeRequestTestCase2 = new CreateVolumeRequest
        {
            Id = string.Empty,
            Title = string.Empty,
            Type = string.Empty,
            Uri = tempDir,
            Capacity = 0,
            Description = "Test Volume1"
        };
        Assert.Throws<RpcException>(() => { _client.CreateVolume(createVolumeRequestTestCase2, superUserOptions); });

        tempDir = $"{@"C:\mirero\TestCase"}/{Path.GetRandomFileName()}";
        var createVolumeRequestTestCase3 = new CreateVolumeRequest
        {
            Id = "1",
            Title = "Title_Volume1",
            Type = "gds",
            Uri = tempDir,
            Capacity = 1000,
            Description = "Test Volume1"
        };
        Assert.Throws<RpcException>(() => { _client.CreateVolume(createVolumeRequestTestCase3, superUserOptions); });
    }
}