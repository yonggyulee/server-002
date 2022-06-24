using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService;

public partial class VolumeServiceTest
{
    [Fact]
    public async Task T02_Update_Volume()
    {
        var volumeName = "TestVolume";
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

        var target = volumes.FirstOrDefault(x => x.Id == volumeName);
        Assert.NotNull(target);
        
        var updateRequest = new UpdateVolumeRequest()
        {
            Id = target.Id,
            Title = $"{target.Title}__Update",
            Uri = $"{target.Uri}_Update",
            Capacity = target.Capacity + 100,
            Description = $"{target.Description}_Update",
            Usage = 1,
        };

        //test를 위해서 디렉토리 정리
        if (!Directory.Exists(target.Uri))
        {
            Directory.CreateDirectory(target.Uri);
        }
        if (Directory.Exists(updateRequest.Uri))
        {
            Directory.Delete(updateRequest.Uri, true);
        }
        
        var updatedVolume = await _volumeServiceClient.UpdateVolumeAsync(updateRequest);
        Assert.Equal(updatedVolume.Title, updateRequest.Title);
        Assert.Equal(updatedVolume.Uri, updateRequest.Uri);
        Assert.Equal(updatedVolume.Capacity, updateRequest.Capacity);
        Assert.Equal(updatedVolume.Description, updateRequest.Description);

        var existUpdatedDir = Directory.Exists(updatedVolume.Uri);
        Assert.True(existUpdatedDir);
    }
}