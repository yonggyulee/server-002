using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.ClassCodeTest;

public partial class ClassCodeServiceTest
{
    [Fact]
    public async void T04_Update_ClassCodeSet()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}