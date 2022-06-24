// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class OdGtTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     private static readonly string GtPath = Path.Combine(DatasetClientTest.GtPath, "ObjectDetection");
//
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         // TestCreateObjectDetectionGt();
//         //TestListObjectDetectionGts();
//         //TestGetObjectDetectionGt();
//         //TestUpdateObjectDetectionGt();
//         TestDeleteObjectDetectionGt();
//     }
//     
//     private static void TestCreateObjectDetectionGt()
//     {
//         var gtList = new List<ObjectDetectionGt>();
//
//         const string localFilename = "2007_000027.xml";
//         
//         const string filename = "image0_object_detection_gt1.xml";
//
//         var uri = Path.Combine(GtPath, localFilename);
//         
//         using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//         var buffer = new byte[fs.Length];
//         fs.Read(buffer, 0, buffer.Length);
//
//         var response = _client.CreateObjectDetectionGt(
//             new CreateObjectDetectionGtRequest
//             {
//                 Filename = filename,
//                 Extension = "xml",
//                 Buffer = ByteString.CopyFrom(buffer),
//                 // Properties = "{ \"key\" : \"test_properties3\" }",
//                 // Description = "Descriptions",
//                 DatasetId = 2,
//                 ImageId = 10,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
//
//     private static void TestListObjectDetectionGts(string listFolderName = "test_object_detection_gt_data1")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         Directory.CreateDirectory(testPath);
//
//         var imgIds = new List<long>() { 1,2,3 };
//         
//         var response = _client.ListObjectDetectionGts(new ListObjectDetectionGtsRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             },
//             LockTimeoutSec = 2,
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//         
//         var gts = response.ObjectDetectionGts;
//         var list = gts.AsEnumerable();
//     
//         var i = 1;
//         foreach (var gt in list)
//         {
//             using var fs = new FileStream(Path.Combine(testPath, "data" + i + "." + gt.Extension), FileMode.Create);
//             fs.Write(gt.Buffer.ToByteArray());
//             gt.Buffer = null;
//             Utils.ToString(gt);
//             i++;
//         }
//     }
//     
//     private static void TestGetObjectDetectionGt(string resultFolderName = "get_object_detection_gt_test")
//     {
//         var testPath = Path.Combine(DownloadFolder, resultFolderName);
//         
//         Directory.CreateDirectory(testPath);
//         
//         var response = _client.GetObjectDetectionGt(new GetObjectDetectionGtRequest
//         {
//             ObjectDetectionGtId = 46,
//             LockTimeoutSec = 2,
//         });
//         
//         using var fs = new FileStream(Path.Combine(testPath, "gt_data." + response.Extension), FileMode.Create);
//         fs.Write(response.Buffer.ToByteArray());
//         response.Buffer = null;
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
//     private static void TestUpdateObjectDetectionGt()
//     {
//         
//         const string extension = "xml";
//         const string localFilename = "2007_000032." + extension;
//         const string filename = "image10_object_detection_gt1_updated4." + extension;
//
//         var fileUri = Path.Combine(GtPath, localFilename);
//         
//         using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//         var buffer = new byte[fs.Length];
//         fs.Read(buffer, 0, buffer.Length);
//         
//         var response = _client.UpdateObjectDetectionGt(
//             new UpdateObjectDetectionGtRequest
//             {
//                 Id = 51,
//                 Filename = filename,
//                 Extension = extension,
//                 Buffer = ByteString.CopyFrom(buffer),
//                 Description = "updated",
//                 DatasetId = 17,
//                 ImageId = 150,
//                 LockTimeoutSec = 2,
//             });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteObjectDetectionGt()
//     {
//         var response = _client.DeleteObjectDetectionGt(
//             new DeleteObjectDetectionGtRequest
//             {
//                 ObjectDetectionGtId = 51,
//                 LockTimeoutSec = 2,
//             });
//         
//         Utils.ToString(response);
//     }
// }