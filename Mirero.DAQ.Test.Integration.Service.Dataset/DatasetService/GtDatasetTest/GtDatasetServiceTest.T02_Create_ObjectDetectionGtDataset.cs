using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Dataset.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Dataset.DatasetService.GtDatasetTest;

public partial class GtDatasetServiceTest
{
    [Fact]
    public async void T02_Create_ObjectDetectionGtDataset()
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

        var createRequest = new CreateObjectDetectionGtDatasetRequest
        {
            Title = "Title_ObjectDetectionGtDataset1",
            DirectoryName = "object_detection_gt_dataset1",
            VolumeId = volume.Id,
            ClassCodeSetId = classCodeSet.Id,
            ImageDatasetId = imageDataset.Id,
            Description = "Test ObjectDetectionGtDataset1 Create."
        };

        try
        {
            var createGtDataset = _client.CreateObjectDetectionGtDataset(createRequest, superUserOptions);
            _output.WriteLine(createGtDataset.ToString());
            Assert.NotNull(createGtDataset);
        }
        catch
        {
            throw;
        }
    }
}