using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T05_List_ClassificationGtDatasets()
    {
        var superUserOptions = await _GetAuthAsync();

        var request = new ListClassificationGtDatasetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 100,
                PageIndex = 0
            }
        };

        var gtDatasets = _client.ListClassificationGtDatasets(request, superUserOptions);
        _output.WriteLine(gtDatasets.ToString());
        Assert.NotNull(gtDatasets);
    }
}