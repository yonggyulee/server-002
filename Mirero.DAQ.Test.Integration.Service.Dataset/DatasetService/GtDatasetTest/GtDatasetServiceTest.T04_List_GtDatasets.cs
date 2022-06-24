using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T04_List_GtDatasets()
    {
        var superUserOptions = await _GetAuthAsync();

        var request = new ListGtDatasetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 100,
                PageIndex = 0
            }
        };

        var gtDatasets = _client.ListGtDatasets(request, superUserOptions);
        _output.WriteLine(gtDatasets.ToString());
        Assert.NotNull(gtDatasets);
    }
}