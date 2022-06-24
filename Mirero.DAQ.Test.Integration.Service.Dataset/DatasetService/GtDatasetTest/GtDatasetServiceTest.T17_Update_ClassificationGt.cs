using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T17_Update_ClassificationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}