using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.VolumeTest;

public partial class VolumeServiceTest
{
    [Fact]
    public async void T02_List_Volumes()
    {
        var superUserOptions = await _GetAuthAsync();

        var listVolumesRequest = new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listVolumes = await _client.ListVolumesAsync(listVolumesRequest, superUserOptions);
        _output.WriteLine(listVolumes.ToString());
        Assert.NotNull(listVolumes.ToString());
    }
}