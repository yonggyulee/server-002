using System.IO;
using System.Linq;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T02_List_Volumes()
    {
        var superUserOptions = await _GetAuthAsync();

        var tempDir = $"{@"C:\mirero\volumes"}/{Path.GetRandomFileName()}";
        var createVolumeRequest = new CreateVolumeRequest
        {
            Id = "2",
            Title = "ListVolumeTest",
            Type = "gds",
            Uri = tempDir,
            Capacity = 10000,
            Description = "ListVolumeTest"
        };
        _client.CreateVolume(createVolumeRequest, superUserOptions);

        var listVolumesRequest = new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listVolumes = await _client.ListVolumesAsync(listVolumesRequest, superUserOptions);

        Assert.NotNull(listVolumes);
        Assert.Equal(listVolumes.PageResult.PageIndex, listVolumesRequest.QueryParameter.PageIndex);
        Assert.Equal(listVolumes.PageResult.PageSize, listVolumesRequest.QueryParameter.PageSize);
        Assert.Equal(listVolumes.Volumes.SingleOrDefault(x => x.Id == createVolumeRequest.Id)?.Id,
            createVolumeRequest.Id);
        Assert.Equal(listVolumes.Volumes.SingleOrDefault(x => x.Id == createVolumeRequest.Id)?.Title,
            createVolumeRequest.Title);
        Assert.Equal(listVolumes.Volumes.SingleOrDefault(x => x.Id == createVolumeRequest.Id)?.Type,
            createVolumeRequest.Type);
        Assert.Equal(listVolumes.Volumes.SingleOrDefault(x => x.Id == createVolumeRequest.Id)?.Uri,
            createVolumeRequest.Uri);
    }
}