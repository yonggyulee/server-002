using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T03_Get_Volume()
    {
        var superUserOptions = await _GetAuthAsync();

        var getVolumeRequest = new GetVolumeRequest
        {
            VolumeId = "ID_Volume1"
        };

        var getVolume = _client.GetVolume(getVolumeRequest, superUserOptions);
        _output.WriteLine(getVolume.ToString());
        Assert.NotNull(getVolume.ToString());
    }
}