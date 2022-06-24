using Grpc.Core;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.ClassCodeTest;

public partial class ClassCodeServiceTest
{
    [Fact]
    public async void T02_List_ClassCodeSets()
    {
        var superUserOptions = await _GetAuthAsync();
    }
}