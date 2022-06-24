using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Update.RcUpdateServices;

public partial class RcUpdateServiceTest
{
    [Fact]
    public async Task T01_List_RcVersion()
    {
        var superUserOptions = await _GetAuthAsync();

        var listRcVersionsRequest = new ListRcSetupVersionsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = "",
            }
        };

        var listRcVersionsResponse = _rcUpdateServiceClient.ListRcSetupVersions(listRcVersionsRequest, superUserOptions);
        _output.WriteLine(listRcVersionsResponse.ToString());
    }
    
}