// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class CGtTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//         
//         // TestCreateClassificationGt();
//         TestListClassificationGts();
//         // TestGetClassificationGt();
//         // TestUpdateClassificationGt();
//         // TestDeleteClassificationGt();
//     }
//     
//     private static void TestCreateClassificationGt()
//     {
//         var response = _client.CreateClassificationGt(
//             new CreateClassificationGtRequest
//             {
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 DatasetId = 1,
//                 ImageId = 1,
//                 ClassCodeId = 1
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListClassificationGts(string listFolderName = "classification_gt_dataset_thumb5")
//     {
//         // var testPath = Path.Combine(DownloadFolder, listFolderName);
//         // if (!Directory.Exists(testPath))
//         // {
//         //     Directory.CreateDirectory(testPath);
//         // }
//
//         var imgIds = new List<long>() { 1,2,3 };
//         
//         var response = _client.ListClassificationGts(new ListClassificationGtsRequest
//         {
//             ClassificationGtDatasetId = 1,
//             ImageIds = { imgIds }
//         });
//         
//         // Console.WriteLine($"PageIndex : {response.PageIndex}");
//         // Console.WriteLine($"PageSize : {response.PageSize}");
//         // Console.WriteLine($"Count : {response.Count}");
//         
//         var gts = response.ClassificationGts;
//         var list = gts.AsEnumerable();
//     
//         var i = 1;
//         foreach (var gt in list)
//         {
//             // using var fs = new FileStream(Path.Combine(testPath, "thumb" + i + ".jpg"), FileMode.Create);
//             // fs.Write(dataset.ThumbnailBuffer.ToByteArray());
//             // dataset.ThumbnailBuffer = null;
//             Utils.ToString(gt);
//             i++;
//         }
//     }
//     
//     private static void TestGetClassificationGt()
//     {
//         var response = _client.GetClassificationGt(new GetClassificationGtRequest
//         {
//             ClassificationGtId = 1
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
//     private static void TestUpdateClassificationGt()
//     {
//         var response = _client.UpdateClassificationGt(
//             new UpdateClassificationGtRequest
//             {
//                 Id = 2,
//                 Description = "updated",
//                 DatasetId = 1,
//                 ImageId = 2,
//                 ClassCodeId = 2
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteClassificationGt()
//     {
//         var response = _client.DeleteClassificationGt(
//             new DeleteClassificationGtRequest
//             {
//                 ClassificationGtId = 3
//             });
//         
//         Utils.ToString(response);
//     }
// }