using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T27_Update_SegmentationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}