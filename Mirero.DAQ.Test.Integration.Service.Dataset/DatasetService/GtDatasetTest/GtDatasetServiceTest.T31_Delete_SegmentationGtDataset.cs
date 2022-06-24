using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T31_Delete_SegmentationGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}