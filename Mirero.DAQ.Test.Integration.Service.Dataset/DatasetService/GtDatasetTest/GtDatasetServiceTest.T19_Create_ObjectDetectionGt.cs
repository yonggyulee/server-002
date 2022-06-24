using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T19_Create_ObjectDetectionGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}