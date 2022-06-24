using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T28_Delete_SegmentationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}