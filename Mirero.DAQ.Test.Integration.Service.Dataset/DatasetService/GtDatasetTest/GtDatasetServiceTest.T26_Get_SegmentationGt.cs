using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T26_Get_SegmentationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}