using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T23_Delete_ObjectDetectionGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}