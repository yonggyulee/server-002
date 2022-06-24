// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class CGtDatasetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//         
//         TestCreateClassificationGtDataset();
//         // TestListDatasets();
//         // TestGetClassificationGtDataset();
//         // TestGetDatasetByName();
//         // TestUpdateClassificationGtDataset();
//         // TestDeleteClassificationGtDataset();
//     }
//
//     private static void TestCreateClassificationGtDataset()
//     {
//         var response = _client.CreateClassificationGtDataset(
//             new CreateClassificationGtDatasetRequest
//             {
//                 Title = "Dataset1_ClassificationGtDataset_1",
//                 DirectoryName = "classification_gt_dataset1",
//                 CreateUser = "이용규",
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 VolumeId = "volume1",
//                 ImageDatasetId = 1,
//                 ClassCodeSetId = 1,
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListDatasets(string listFolderName = "classification_gt_dataset_thumb5")
//     {
//         // var testPath = Path.Combine(DownloadFolder, listFolderName);
//         // if (!Directory.Exists(testPath))
//         // {
//         //     Directory.CreateDirectory(testPath);
//         // }
//         
//         var response = _client.ListClassificationGtDatasets(new ListClassificationGtDatasetsRequest
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
//         var datasets = response.ClassificationGtDatasets;
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
//     private static void TestGetClassificationGtDataset()
//     {
//         var response = _client.GetClassificationGtDataset(new GetClassificationGtDatasetRequest
//         {
//             ClassificationGtDatasetId = 2
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
//     private static void TestUpdateClassificationGtDataset()
//     {
//         var response = _client.UpdateClassificationGtDataset(
//             new UpdateClassificationGtDatasetRequest
//             {
//                 Id = 2,
//                 Title = "CGtDataset1_updated",
//                 VolumeId = "volume2",
//                 ImageDatasetId = 10,
//                 ClassCodeSetId = 4,
//                 UpdateUser = "김철수",
//                 Description = "updated",
//                 DirectoryName = "c_gt_dataset1_updated"
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteClassificationGtDataset()
//     {
//         var response = _client.DeleteClassificationGtDataset(
//             new DeleteClassificationGtDatasetRequest
//             {
//                 ClassificationGtDatasetId = 3
//             });
//         
//         Utils.ToString(response);
//     }
// }