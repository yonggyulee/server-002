// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class OdGtDatasetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         //TestCreateObjectDetectionGtDataset();
//         // TestListDatasets();
//         // TestGetObjectDetectionGtDataset();
//         //TestUpdateObjectDetectionGtDataset();
//         TestDeleteObjectDetectionGtDataset();
//     }
//
//     private static void TestCreateObjectDetectionGtDataset()
//     {
//         var response = _client.CreateObjectDetectionGtDataset(
//             new CreateObjectDetectionGtDatasetRequest
//             {
//                 Title = "ObjectDetectionGtDataset1",
//                 DirectoryName = "object_detection_gt_dataset1",
//                 CreateUser = "이용규",
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 VolumeId = "volume1",
//                 ImageDatasetId = 2,
//                 ClassCodeSetId = 2
//                 // CreateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)),
//                 // UpdateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListDatasets(string listFolderName = "object_detection_gt_dataset_thumb5")
//     {
//         // var testPath = Path.Combine(DownloadFolder, listFolderName);
//         // if (!Directory.Exists(testPath))
//         // {
//         //     Directory.CreateDirectory(testPath);
//         // }
//         
//         var response = _client.ListObjectDetectionGtDatasets(new ListObjectDetectionGtDatasetsRequest
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
//         var datasets = response.ObjectDetectionGtDatasets;
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
//     private static void TestGetObjectDetectionGtDataset()
//     {
//         var response = _client.GetObjectDetectionGtDataset(new GetObjectDetectionGtDatasetRequest
//         {
//             ObjectDetectionGtDatasetId = 1
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
//     private static void TestUpdateObjectDetectionGtDataset()
//     {
//         var response = _client.UpdateObjectDetectionGtDataset(
//             new UpdateObjectDetectionGtDatasetRequest
//             {
//                 Id = 14,
//                 Title = "Dataset5_OdGtDataset5_updated",
//                 VolumeId = "volume1",
//                 ImageDatasetId = 19,
//                 ClassCodeSetId = 14,
//                 UpdateUser = "김철수",
//                 Description = "updated",
//                 DirectoryName = "dataset5_od_gt_dataset5_updated",
//                 LockTimeoutSec = 2,
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteObjectDetectionGtDataset()
//     {
//         var response = _client.DeleteObjectDetectionGtDataset(
//             new DeleteObjectDetectionGtDatasetRequest
//             {
//                 ObjectDetectionGtDatasetId = 14,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
// }