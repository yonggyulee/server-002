using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.ClassCodeTest;

public partial class ClassCodeServiceTest
{
    [Fact]
    public async void T06_Create_ClassCode()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}