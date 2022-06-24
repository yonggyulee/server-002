using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T08_List_ObjectDetectionGtDatasets()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}