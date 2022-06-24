using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T15_List_ClassificationGts()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}