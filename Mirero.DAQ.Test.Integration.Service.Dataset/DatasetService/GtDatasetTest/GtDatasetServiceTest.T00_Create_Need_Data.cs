using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T00_Create_Need_Data()
    {
        var superUserOptions = await _GetAuthAsync();
        //_testDataGenerator.VolumeDataGenerate(1);
        //_testDataGenerator.DatasetDataGenerate(1);
        //_testDataGenerator.SampleDataGenerate(3, 3);
        //_testDataGenerator.ClassCodeSetGenerate(3);
        //_testDataGenerator.ClassCodeDataGenerate(3, 3);
    }
}