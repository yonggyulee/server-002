using System.IO;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.VolumeService;

public partial class VolumeServiceTest
{
    [Fact]
    public async Task T01_Create_Volume()
    {
        var volumeName = "Volume1";
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
    }
}