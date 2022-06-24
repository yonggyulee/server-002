using Grpc.Core;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService;

public partial class VolumeServiceTest
{

    [Fact]
    public async Task T04_Create_Get_Delete_Volume()
    {
        var volumeName = "Volume2";
        var createRequest = new CreateVolumeRequest
        {
            Id = volumeName,
            Title = $"Title_{volumeName}",
            Uri = $"{_fixture.VolumeBaseUri}/{volumeName}",
            Capacity = 100000000,
            Description = $"Test {volumeName}",
            Usage = 1,
        };
        
        //test를 위해서 있음면 지움
        if (Directory.Exists(createRequest.Uri))
        {
            Directory.Delete(createRequest.Uri, true);
        }
        
        var createdVolume = await _volumeServiceClient.CreateVolumeAsync(createRequest);
        Assert.NotNull(createdVolume);
        Assert.Equal(createRequest.Id, createdVolume.Id);
        Assert.Equal(createRequest.Title, createdVolume.Title);
        Assert.Equal(createRequest.Uri, createdVolume.Uri);
        Assert.Equal(createRequest.Capacity, createdVolume.Capacity);
        Assert.Equal(createRequest.Description, createdVolume.Description);
        Assert.Equal(createRequest.Usage, createdVolume.Usage);
            
        var existDir = Directory.Exists(createdVolume.Uri);
        Assert.True(existDir);
      

        var listRequest = new ListVolumesRequest
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{volumeName}\""
            }
        };

   
        var volumes = (await _volumeServiceClient.ListVolumesAsync(listRequest)).Volumes;
        Assert.NotEmpty(volumes);

        var target = volumes.FirstOrDefault(x => x.Id == createRequest.Id);
        Assert.NotNull(target);
        Assert.Equal(createRequest.Id, target.Id);
        Assert.Equal(createRequest.Title, target.Title);
        Assert.Equal(createRequest.Uri, target.Uri);
        Assert.Equal(createRequest.Capacity, target.Capacity);
        Assert.Equal(createRequest.Description, target.Description);
        Assert.Equal(createRequest.Usage, target.Usage);

        await _volumeServiceClient.DeleteVolumeAsync(new DeleteVolumeRequest() { VolumeId = target.Id });

        var refreshVolumes = (await _volumeServiceClient.ListVolumesAsync(listRequest)).Volumes;
        Assert.Empty(refreshVolumes);
        
        var notExistDir = Directory.Exists(createdVolume.Uri);
        Assert.False(notExistDir);
    }
}
