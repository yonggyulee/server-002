using System.IO;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService;

public partial class VolumeServiceTest
{
    [Fact]
    public async Task T05_Create_Update_Delete_Volume()
    {
        var volumeName = "Volume3";
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
        var existDir = Directory.Exists(createdVolume.Uri);
        Assert.True(existDir);

        var updateRequest = new UpdateVolumeRequest()
        {
            Id = createdVolume.Id,
            Title = $"{createdVolume.Title}__Update",
            Uri = $"{createdVolume.Uri}_Update",
            Capacity = createdVolume.Capacity + 100,
            Description = $"{createdVolume.Description}_Update",
            Usage = 1,
        };

        var updatedVolume = await _volumeServiceClient.UpdateVolumeAsync(updateRequest);
        Assert.Equal(updatedVolume.Title, updateRequest.Title);
        Assert.Equal(updatedVolume.Uri, updateRequest.Uri);
        Assert.Equal(updatedVolume.Capacity, updateRequest.Capacity);
        Assert.Equal(updatedVolume.Description, updateRequest.Description);
        var existUpdatedDir = Directory.Exists(updatedVolume.Uri);
        Assert.True(existUpdatedDir);
        
        await _volumeServiceClient.DeleteVolumeAsync(new DeleteVolumeRequest() { VolumeId = updatedVolume.Id });
        existUpdatedDir = Directory.Exists(updatedVolume.Uri);
        Assert.False(existUpdatedDir);
    }
}