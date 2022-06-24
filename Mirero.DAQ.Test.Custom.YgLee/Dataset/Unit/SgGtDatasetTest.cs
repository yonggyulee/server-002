// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class SgGtDatasetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         //TestCreateSegmentationGtDataset();
//         // TestListDatasets();
//         // TestGetSegmentationGtDataset();
//         //TestUpdateSegmentationGtDataset();
//         TestDeleteSegmentationGtDataset();
//     }
//
//     private static void TestCreateSegmentationGtDataset()
//     {
//         var response = _client.CreateSegmentationGtDataset(
//             new CreateSegmentationGtDatasetRequest
//             {
//                 Title = "SegmentationGtDataset3",
//                 DirectoryName = "segmentation_gt_dataset3",
//                 CreateUser = "이용규",
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 VolumeId = "volume1",
//                 ImageDatasetId = 3,
//                 ClassCodeSetId = 3
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListDatasets(string listFolderName = "segmentation_gt_dataset_thumb5")
//     {
//         // var testPath = Path.Combine(DownloadFolder, listFolderName);
//         // if (!Directory.Exists(testPath))
//         // {
//         //     Directory.CreateDirectory(testPath);
//         // }
//         
//         var response = _client.ListSegmentationGtDatasets(new ListSegmentationGtDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             }
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var datasets = response.SegmentationGtDatasets;
//         var list = datasets.AsEnumerable();
//     
//         var i = 1;
//         foreach (var dataset in list)
//         {
//             // using var fs = new FileStream(Path.Combine(testPath, "thumb" + i + ".jpg"), FileMode.Create);
//             // fs.Write(dataset.ThumbnailBuffer.ToByteArray());
//             // dataset.ThumbnailBuffer = null;
//             Utils.ToString(dataset);
//             i++;
//         }
//     }
//     
//     private static void TestGetSegmentationGtDataset()
//     {
//         var response = _client.GetSegmentationGtDataset(new GetSegmentationGtDatasetRequest
//         {
//             SegmentationGtDatasetId = 3
//         });
//
//         Utils.ToString(response);
//     }
//     
//     // private static void TestGetImageDatasetByName()
//     // {
//     //     var response = _client.GetImageDatasetByName(new GetImageDatasetByNameRequest
//     //     {
//     //         Name = "dataset2"
//     //     });
//     //
//     //     Utils.ToString(response);
//     // }
//     
//     private static void TestUpdateSegmentationGtDataset()
//     {
//         var response = _client.UpdateSegmentationGtDataset(
//             new UpdateSegmentationGtDatasetRequest
//             {
//                 Id = 15,
//                 Title = "Dataset6_SgGtDataset6_updated",
//                 VolumeId = "volume1",
//                 ImageDatasetId = 20,
//                 ClassCodeSetId = 15,
//                 UpdateUser = "김철수",
//                 Description = "updated",
//                 DirectoryName = "dataset_6_sg_gt_dataset6_updated",
//                 LockTimeoutSec = 2,
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteSegmentationGtDataset()
//     {
//         var response = _client.DeleteSegmentationGtDataset(
//             new DeleteSegmentationGtDatasetRequest
//             {
//                 SegmentationGtDatasetId = 15,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
// }