using System.IO;
using System.Linq;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.GdsTest.Gds;

public partial class GdsServiceTest
{
    [Fact]
    public async void T02_List_Gds()
    {
        var superUserOption = await _GetAuthAsync();
        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "ListGdsVolumeId",
            Title = "ListGdsTestVolume",
            Type = "gds",
            Uri = tempDir,
            Capacity = 100000000,
            Description = "Test Volume"
        };

        _volume.CreateVolume(createVolumeRequest, superUserOption);

        var createGdsRequest = new CreateGdsRequest
        {
            VolumeId = "ListGdsVolumeId",
            Filename = "ListGdsTestFileName",
            Extension = "gds",
            Properties = "1",
            Description = "CreateGds"
        };

        var testCreateGds = _client.CreateGds(createGdsRequest, superUserOption);
        var listGdsesRequest = new ListGdsesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listGdses = await _client.ListGdsesAsync(listGdsesRequest, superUserOption);
        Assert.NotNull(listGdsesRequest);
        Assert.Equal(listGdses.PageResult.PageIndex, listGdsesRequest.QueryParameter.PageIndex);
        Assert.Equal(listGdses.PageResult.PageSize, listGdsesRequest.QueryParameter.PageSize);

        var updatedGdses = listGdses.Gdses.SingleOrDefault(x => x.Id == testCreateGds.GdsId)!;
        Assert.NotNull(listGdses);
        Assert.Equal(createGdsRequest.VolumeId, updatedGdses.VolumeId);
        Assert.Equal(createGdsRequest.Filename, updatedGdses.Filename);
        Assert.Equal(createGdsRequest.Extension, updatedGdses.Extension);
        Assert.Equal(createGdsRequest.Properties, updatedGdses.Properties);
        Assert.Equal(createGdsRequest.Description, updatedGdses.Description);
    }
}