using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T30_Delete_ObjectDetectionGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}