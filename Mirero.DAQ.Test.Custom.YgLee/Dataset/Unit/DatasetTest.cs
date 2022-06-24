// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class DatasetTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         //TestCreateDataset();
//         TestListDatasets();
//         //TestGetImageDataset();
//         //TestUpdateImageDataset();
//         // TestDeleteDataset();
//     }
//
//     private static void TestCreateDataset()
//     {
//         var response = _client.CreateImageDataset(new CreateImageDatasetRequest
//         {
//             Title = "test_12345678901",
//             DirectoryName = "test_12345678901",
//             CreateUser = "test_user1",
//             Properties = "{ \"count\" : 80 }",
//             Description = "classification dataset",
//             VolumeId = "volume1",
//             // CreateDate = Timestamp.FromDateTime(DateTime.SpecifyKind(
//             //     DateTime.ParseExact("20220123153247","yyyyMMddHHmmss",System.Globalization.CultureInfo.InvariantCulture), 
//             //     DateTimeKind.Utc)),
//             // UpdateDate = Timestamp.FromDateTime(
//             //     DateTime.SpecifyKind(
//             //         DateTime.ParseExact("20220123153247","yyyyMMddHHmmss",System.Globalization.CultureInfo.InvariantCulture), 
//             //         DateTimeKind.Utc))
//         });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListDatasets(string listFolderName = "image_dataset_thumb12345")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         if (!Directory.Exists(testPath))
//         {
//             Directory.CreateDirectory(testPath);
//         }
//         
//         var response = _client.ListImageDatasets(new ListImageDatasetsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             },
//             WithThumbnail = true,
//             LockTimeoutSec = 2,
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var datasets = response.Datasets;
//         var list = datasets.AsEnumerable();
//     
//         var i = 1;
//         foreach (var dataset in list)
//         {
//             using var fs = new FileStream(Path.Combine(testPath, "thumb" + i + ".jpg"), FileMode.Create);
//             fs.Write(dataset.ThumbnailBuffer.ToByteArray());
//             dataset.ThumbnailBuffer = null;
//             Utils.ToString(dataset);
//             i++;
//         }
//     }
//     
//     private static void TestGetImageDataset()
//     {
//         var response = _client.GetImageDataset(new GetImageDatasetRequest
//         {
//             DatasetId = 16
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
//     private static void TestUpdateImageDataset()
//     {
//         var response = _client.UpdateImageDataset(new UpdateImageDatasetRequest
//         {
//             Id = 15,
//             Title = "test_123456789_updated",
//             VolumeId = "volume1",
//             UpdateUser = "이용규",
//             Description = "updated",
//             DirectoryName = "test_123456789_updated",
//             LockTimeoutSec = 2,
//         });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteImageDataset()
//     {
//         var response = _client.DeleteImageDataset(new DeleteImageDatasetRequest
//         {
//             DatasetId = 12,
//             LockTimeoutSec = 2,
//         });
//         
//         Utils.ToString(response);
//     }
// }