using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T03_Create_SegmentationGtDataset()
    {
        var superUserOptions = await _GetAuthAsync();
        var volume = _volumeServiceClient.ListVolumes(new ListVolumesRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 1,
                PageIndex = 0
            }
        }).Volumes.First();
        Assert.NotNull(volume);

        var classCodeSet = _classCodeServiceClient.ListClassCodeSets(new ListClassCodeSetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 1,
                PageIndex = 0
            }
        }).ClassCodeSets.First();
        Assert.NotNull(classCodeSet);

        var imageDataset = _imageDatasetServiceClient.ListImageDatasets(new ListImageDatasetsRequest
        {
            QueryParameter = new QueryParameter
            {
                PageSize = 1,
                PageIndex = 0
            }
        }).Datasets.First();
        Assert.NotNull(imageDataset);

        var createRequest = new CreateSegmentationGtDatasetRequest
        {
            Title = "Title_SegmentationGtDataset1",
            DirectoryName = "segmentation_gt_dataset1",
            VolumeId = volume.Id,
            ClassCodeSetId = classCodeSet.Id,
            ImageDatasetId = imageDataset.Id,
            Description = "Test SegmentationGtDataset1 Create."
        };

        try
        {
            var createGtDataset = _client.CreateSegmentationGtDataset(createRequest, superUserOptions);
            _output.WriteLine(createGtDataset.ToString());
            Assert.NotNull(createGtDataset);
        }
        catch
        {
            throw;
        }
    }
}