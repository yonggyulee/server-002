using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T16_Get_ClassificationGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}