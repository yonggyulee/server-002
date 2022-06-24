using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T20_List_ObjectDetectionGts()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}