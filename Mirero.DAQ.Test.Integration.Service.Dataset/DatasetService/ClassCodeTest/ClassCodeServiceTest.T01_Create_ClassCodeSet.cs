using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.ClassCodeTest;

public partial class ClassCodeServiceTest
{
    [Fact]
    public async void T01_Create_ClassCodeSet()
    {
        var superUserOptions = await _GetAuthAsync();
        
        var createClassCodeSetRequest = new CreateClassCodeSetRequest()
        {
            Title = "Title_ClassCodeSet001",
            DirectoryName = "test_class_code_set001",
            VolumeId = "volume1",
            Task = "classification",
            Description = "Test ClassCodeSet1"
        };

        try
        {
            var createClassCodeSet = _client.CreateClassCodeSet(createClassCodeSetRequest, superUserOptions);
            _output.WriteLine(createClassCodeSet.ToString());
            Assert.NotNull(createClassCodeSet);
        }
        catch
        {
            throw;
        }
    }
}
