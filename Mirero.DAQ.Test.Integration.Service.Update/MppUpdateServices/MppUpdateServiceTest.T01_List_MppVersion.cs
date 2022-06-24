using Mirero.DAQ.Domain.Update.Protos.V1;
using Xunit;
using QueryParameter = Mirero.DAQ.Domain.Common.Protos.QueryParameter;

namespace Mirero.DAQ.Test.Integration.Service.Update.MppUpdateServices;

public partial class MppUpdateServiceTest
{
    [Fact]
    public async Task T01_List_MppVersion()
    {
        await _fixture.SignInAsync("administrator", "mirero2816!");
        var superUserOptions = _fixture.OptionsWithAuthHeader();

        var listMppVersionsRequest = new ListMppSetupVersionsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10,
                Where = "",
            }
        };

       var listMppVersionsResponse = _mppUpdateServiceClient.ListMppSetupVersions(listMppVersionsRequest, superUserOptions);
       _output.WriteLine(listMppVersionsResponse.ToString());
    }
}