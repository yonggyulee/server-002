using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService;

public partial class VolumeServiceTest
{
    [Fact]
    public async Task T03_Delete_Volume()
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
        //test를 위해서 없으면 생성함
        if (!Directory.Exists(target.Uri))
        {
            Directory.CreateDirectory(target.Uri);
        }
        
        await _volumeServiceClient.DeleteVolumeAsync(new DeleteVolumeRequest() { VolumeId = target.Id });

        var refreshVolumes = (await _volumeServiceClient.ListVolumesAsync(listRequest)).Volumes;
        Assert.Empty(refreshVolumes);
        
        var notexistDir = Directory.Exists(target.Uri);
        Assert.False(notexistDir);
    }
}