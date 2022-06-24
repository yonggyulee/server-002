using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T21_Get_ObjectDetectionGt()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}