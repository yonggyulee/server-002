using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T24_Create_SegmentationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}