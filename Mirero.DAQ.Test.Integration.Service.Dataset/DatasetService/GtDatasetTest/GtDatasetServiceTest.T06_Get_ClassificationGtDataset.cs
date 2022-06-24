using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T06_Get_ClassificationGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();

        var gtDataset = _client.ListClassificationGtDatasets(new ListClassificationGtDatasetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 1,
                PageIndex = 0
            }
        }, superUserOptions).ClassificationGtDatasets.First();

        var request = new GetClassificationGtDatasetRequest
        {
            ClassificationGtDatasetId = gtDataset.Id
        };

        var response = _client.GetClassificationGtDataset(request, superUserOptions);
        _output.WriteLine(response.ToString());
        Assert.NotNull(response);
        Assert.Equal(gtDataset.Id, response.Id);
        Assert.Equal(gtDataset.Title, response.Title);
        Assert.Equal(gtDataset.DirectoryName, response.DirectoryName);
        Assert.Equal(gtDataset.CreateDate, response.CreateDate);
        Assert.Equal(gtDataset.UpdateDate, response.UpdateDate);
        Assert.Equal(gtDataset.CreateUser, response.CreateUser);
        Assert.Equal(gtDataset.UpdateUser, response.UpdateUser);
        Assert.Equal(gtDataset.VolumeId, response.VolumeId);
        Assert.Equal(gtDataset.ImageDatasetId, response.ImageDatasetId);
        Assert.Equal(gtDataset.ClassCodeSetId, response.ClassCodeSetId);
        Assert.Equal(gtDataset.Properties, response.Properties);
        Assert.Equal(gtDataset.Description, response.Description);
    }
}