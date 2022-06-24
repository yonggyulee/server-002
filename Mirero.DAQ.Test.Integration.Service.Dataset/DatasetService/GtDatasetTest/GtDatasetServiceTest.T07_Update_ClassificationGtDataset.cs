using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T07_Update_ClassificationGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}