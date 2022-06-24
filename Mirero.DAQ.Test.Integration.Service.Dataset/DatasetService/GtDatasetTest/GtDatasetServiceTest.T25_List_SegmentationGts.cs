using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T25_List_SegmentationGts()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}