using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T09_Get_ObjectDetectionGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}